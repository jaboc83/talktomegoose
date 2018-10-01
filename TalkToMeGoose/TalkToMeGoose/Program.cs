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

namespace TalkToMeGoose
{
    class Program
    {
        static readonly List<Phrase> phrases = new List<Phrase>();
        static Stopwatch runningTime = new Stopwatch();
        static Configuration cfg = new Configuration();

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
                        InactivateTimeInMin = fields.Length > 3 && !string.IsNullOrWhiteSpace(fields[3]) ? int.Parse(fields[3]) : (int?)null
                    });
                }
            }

            // Read in config
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                cfg = JsonConvert.DeserializeObject<Configuration>(json);
            }

            Dictionary<string, int> countCheck = new Dictionary<string, int>();
            SpeechSynthesizer synthesizer = new SpeechSynthesizer
            {
                Volume = cfg.Volume,  // 0...100
                Rate = cfg.SpeechRate     // -10...10
            };
            synthesizer.SelectVoice(synthesizer.GetInstalledVoices().OrderBy(x => Guid.NewGuid()).FirstOrDefault().VoiceInfo.Name);
            var selector = new DynamicRandomSelector<Phrase>();
            foreach(var phrase in phrases)
            {
                countCheck.Add(phrase.Message, 0);
                selector.Add(phrase, phrase.Weight);
            }
            selector.Build();
            bool isValidSelection;
            Phrase selection = null;
            while (true)
            {
                // Choose a random voice
                if (cfg.RandomizeVoice)
                {
                    synthesizer.SelectVoice(synthesizer.GetInstalledVoices().OrderBy(x => Guid.NewGuid()).FirstOrDefault().VoiceInfo.Name);
                }
                isValidSelection = false;
                while (!isValidSelection) {
                    selection = selector.SelectRandomItem();
                    if (runningTime.Elapsed.TotalMinutes > selection.ActivateTimeInMin &&
                        (selection.InactivateTimeInMin == null || runningTime.Elapsed.TotalMinutes < selection.InactivateTimeInMin)){
                        isValidSelection = true;
                    }
                }
                Console.WriteLine(selection);
                synthesizer.Speak(selection.Message);
                Thread.Sleep(cfg.IntervalInSec * 1000);
            }
        }
    }
}

