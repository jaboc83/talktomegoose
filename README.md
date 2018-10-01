# Talk to Me Goose (TTMG)
A League of Legends training tool

## What is TTMG?
This is a simple application for helping you improve your League of Legends skills. The idea is simple, have someone hinting at you to remember to do important things throughout the game. Many people have recommended having a metronome clicking in the background to teach to you look at the mini-map. I've taken that idea a bit further to allow you to have your computer speak a custom set of phrases to you as you play the game to remind you to do many other important things while you play.

## How to Install and Run TTMG
To install the tool simply download the zip located here: xxx and unzip somewhere on your machine. Then double click the talktomegoose.exe to run the application. This will open a small console window like the one shown in the following picture. As time goes on the computer will read phrases to you and will also print the phrase to the small dialog window if you want to look back at what was said.
 [picture]
 
 Be sure to start the application about when the game actually starts or your time based rules might be inaccurate. (See Active and Inactive rule settings in the phrases.csv section below for more detail)
 
## How to Configure TTMG
The application comes with a pre-set configuration that uses the set of phrases I've configured for myself, but I encourage you to devise and share with others your own set of phrases for your skill level and role.

There are two files that control how the application works and I will outline the differences below. If you make changes to the files you will need to restart the application to apply the changes.
### config.json
This file controls overall settings of Talk to Me Goose and will likely not be changed much after the first time you set it. It has the following settings:
* **intervalInSec:** This setting tells TTMG how often to send you hints. the default is 10 seconds, but if you would like to make that more or less frequent adjust the number here.
* **randomizeVoice:** By default the application will randomly select a new voice from those installed on your system each time it reads a phrase. This is to make the voice a little harder to ignore, but if you prefer to have a single voice the entire time you can set this to false and it will select a single voice for the entire game.
* **volume:** (0 - 100) By default the volume is set to 100%, but if you would like the voice to be more in the background you can adjust it here.
* **speechRate:** (-10 - 10) By default the rate of speech is set to 0 (normal) but if you want it slower or faster you can adjust that here. -10 is the slowest and 10 is the fastest.

### phrases.csv
This file controls what TTMG says, how often it says it, and when it starts and stops using a phrase.
The file is formated as a comma-seperated-value file which can be opened with Excel, or Google sheets if you like, or even just notepad or any other text editor. The format is as follows:
* Each line represents a phrase that TTMG will use
* Each value related to the phrase in a line is seperated by a comma (hence the name comma-seperated-value)
* You can edit the sample phrases.csv if you like or create a copy to edit, but the file that the application uses will be the one named phrases.csv and it must be in the same folder as the talktomegoose.exe file.
* The values must follow the order outlined below.
  * **Message:** This is the phrase that you would like to hear. e.g. "Check the mini map."
  * **Weight:** This number represents how often you want this phrase to be read. The numbers are relative to each other so it doesn't matter if they are 1, 100, or 1,000. Larger numbers will be read more often than smaller numbers. You can think of it like entering names into a lottery, the more names you enter the more likely you are to win. 
  * **Activate after x min:** This number represents the number of minutes you want to wait before the game starts reading this phrase. For example: if you want to be prompted about baron related things you probably don't want to hear about it in the first 15 min of the game so you can set the activate time closer to when the baron would come into play. A setting of 0 means it is active immediately when you start.
  * **Inactivate after x min:** This number is the opposite of the above value. It represents the number of minutes after which you no longer want to hear about something. For instance it makes no sense to ask questions about rift harold after a certain point in the game so you can inactivate the rule after 20 minutes if you like. Leaving the value blank means the rule runs forever.

## Future improvements
Please feel free to send your ideas to improve the tool and any phrase sets you create that would be helpful to other users of the tool.
* Create a web based version?
* Create a nice interface for setting up the rules instead of editing config files.
* Create a collection of default phrases.csv for different roles
* Allow users to select phrase set at run time
* Allow Users to change settings on the fly
* See if there is a way to start the app when the game starts automatically
