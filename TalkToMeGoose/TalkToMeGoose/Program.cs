using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using DataStructures.RandomSelector;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Media;
using NAudio.Wave;

namespace TalkToMeGoose
{
    class Program
    {
        static readonly List<Phrase> phrases = new List<Phrase>();
        static Stopwatch runningTime = new Stopwatch();
        static Configuration cfg = new Configuration();

        /// <summary>
        /// Main application logic
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // keep a running timer for phrase activation/inactivation
            runningTime.Start();
            
            // Read in the phrases
            using (TextFieldParser csvParser = new TextFieldParser("phrases.csv"))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    phrases.Add(new Phrase
                    {
                        Message = fields[0],
                        Weight = int.Parse(fields[1]),
                        ActivateTimeInMin = fields.Length > 2 && !string.IsNullOrWhiteSpace(fields[2]) ? int.Parse(fields[2]) : 0,
                        InactivateTimeInMin = fields.Length > 3 && !string.IsNullOrWhiteSpace(fields[3]) ? int.Parse(fields[3]) : (int?)null,
                        AudioFile = fields.Length > 4 && !string.IsNullOrWhiteSpace(fields[4]) ? fields[4] : null
                    });
                }
            }

            // Read in config
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                cfg = JsonConvert.DeserializeObject<Configuration>(json);
            }

            // Setup voice
            Dictionary<string, int> countCheck = new Dictionary<string, int>();
            SpeechSynthesizer synthesizer = new SpeechSynthesizer
            {
                Volume = cfg.Volume,  // 0...100
                Rate = cfg.SpeechRate     // -10...10
            };
            // Chose the voice once here in case they don't want random voice option
            synthesizer.SelectVoice(synthesizer.GetInstalledVoices().OrderBy(x => Guid.NewGuid()).FirstOrDefault().VoiceInfo.Name);

            // Build the random phrase selector
            var selector = new DynamicRandomSelector<Phrase>();
            foreach(var phrase in phrases)
            {
                countCheck.Add(phrase.Message, 0);
                selector.Add(phrase, phrase.Weight);
            }
            selector.Build();

            bool isValidSelection;
            Phrase selection = null;
            // Begin speaking
            while (true)
            {
                // Choose a random voice if needed
                if (cfg.RandomizeVoice)
                {
                    synthesizer.SelectVoice(synthesizer.GetInstalledVoices().OrderBy(x => Guid.NewGuid()).FirstOrDefault().VoiceInfo.Name);
                }
                isValidSelection = false;

                // Make sure the selected phrase is valid for the current game time
                while (!isValidSelection) {
                    selection = selector.SelectRandomItem();
                    if (runningTime.Elapsed.TotalMinutes > selection.ActivateTimeInMin &&
                        (selection.InactivateTimeInMin == null || runningTime.Elapsed.TotalMinutes < selection.InactivateTimeInMin)){
                        isValidSelection = true;
                    }
                }

                // Print the phrase
                Console.WriteLine(selection);
                // Speak the phrase
                if (selection.AudioFile != null)
                {
                    using (var audioFile = new AudioFileReader(selection.AudioFile))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();
                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(500);
                        }
                    }
                }
                else
                {
                    synthesizer.Speak(selection.Message);
                }

                // Wait before speaking the next phrase
                Thread.Sleep(cfg.IntervalInSec * 1000);
            }
        }
    }
}

