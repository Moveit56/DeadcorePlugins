﻿// system imports for base functionality
using System;
using System.Collections.Generic;
// library reference imports
using UnityEngine;
using DCPMCommon.Interfaces;
using DCPMCommon;
using System.Diagnostics;
using LevelCoordinates;
using System.Text;
// class 
public class VelocityMeter : MonoBehaviour, LoadablePlugin
{
    KeyCode timerButton;
    KeyCode resetButton;
    KeyCode revertKey;

    private Stopwatch LevelTimer;

    //Create the pluginInfo dictionary
    Dictionary<String, String> pluginInfo = new Dictionary<String, String>()
    {
        { "Name",   "Timer" },
        { "Author", "Conrad Weiser" },
        { "Ver",    "0.1" },
        { "Desc",   "Adds a stopwatch on the topleft." }
    };

    /*
     _                 _      _    _     ___ _           _        ___     _            __             
    | |   ___  __ _ __| |__ _| |__| |___| _ \ |_  _ __ _(_)_ _   |_ _|_ _| |_ ___ _ _ / _|__ _ __ ___ 
    | |__/ _ \/ _` / _` / _` | '_ \ / -_)  _/ | || / _` | | ' \   | || ' \  _/ -_) '_|  _/ _` / _/ -_)
    |____\___/\__,_\__,_\__,_|_.__/_\___|_| |_|\_,_\__, |_|_||_| |___|_||_\__\___|_| |_| \__,_\__\___|
                                                |___/                                              
    */

    //Destroy this script component and any other script components that this plugin may have created
    public void Unload()
    {
        LogMessage("Unloading plugin");
        GameObject.Destroy(this);
    }

    public Dictionary<String, String> GetPluginInfo()
    {
        return pluginInfo;
    }

    /*
     _   _      _ _          __  __     _   _            _    
    | | | |_ _ (_) |_ _  _  |  \/  |___| |_| |_  ___  __| |___
    | |_| | ' \| |  _| || | | |\/| / -_)  _| ' \/ _ \/ _` (_-<
     \___/|_||_|_|\__|\_, | |_|  |_\___|\__|_||_\___/\__,_/__/
                      |__/                                    
    */

    void Awake()
    {
        LogMessage("=== Creating Stopwatch links ===");
        //Check or create settings
        //Alternatively could use the DeadCore SettingsManager however this encrypts and serializes the data to settings.save and is not easily changed
        timerButton = DCPMSettings.GetKeyCodeSetting("DCPM-ToggleTimer", KeyCode.Q);
        revertKey = DCPMSettings.GetKeyCodeSetting("DCPM-RevertCheckpoint", KeyCode.R);
        resetButton = DCPMSettings.GetKeyCodeSetting("DCPM-ResetButton", KeyCode.F5);

        DCPMMainConsole.Instance.ConsoleInput += ConsoleInput;
        LevelTimer = new Stopwatch();

        LogMessage("=== Stopwatch init complete ===");
    }

    private void ConsoleInput(String[] args)
    {
        if (args.Length >= 1 && args[0] == "timer")
        {
            if (args.Length >= 2 && args[1] == "bind")
            {
                if (args.Length >= 3)
                {
                    try
                    {
                        timerButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), args[2]);
                        DCPMSettings.SetSetting("DCPM-ToggleTimer", timerButton);
                    }
                    catch (Exception)
                    {
                        LogMessage("Error: Cannot convert {0} to UnityEngine.KeyCode", args[2]);
                    }
                }
                else
                {
                    LogMessage("'DCPM-ToggleTimer' = '{0}'", timerButton);
                }
            }
        }

        // console command to output the current coordinates
        if (args.Length == 1 && args[0] == "coord") {
            LogMessage("=== Current Coordinates ===");
            LogMessage("= X ==> " + Math.Round(Android.Instance.gameObject.transform.rigidbody.position.x));
            LogMessage("= Y ==> " + Math.Round(Android.Instance.gameObject.transform.rigidbody.position.y));
            LogMessage("= Z ==> " + Math.Round(Android.Instance.gameObject.transform.rigidbody.position.z));
        }

    }

    
    //Put stuff that you would normally put in the corresponding Unity method in the following methods
    //This is called once per frame
    private int counter = 0, index = 0;
    private Vector3 currentPosition = new Vector3();
    private List<LevelCoordinate> LevelCoordinates = null;
    private LevelCoordinateIF LevelCoordinateManager = null;
    void Update() {
        
        // handle when the reset key is pressed
        if (Input.GetKeyDown(resetButton)) {
            // remove the references to the level coordinate objects
            this.LevelCoordinates = null;
            this.LevelCoordinateManager = null;
            // reload the level
            Application.LoadLevel(Application.loadedLevel);
            // stop and reset the timer
            this.LevelTimer.Stop();
            this.LevelTimer.Reset();
            // LogMessage("Loaded: {0} - Having an index of {1}", Application.loadedLevelName, Application.loadedLevel);
        }

        // check if the timer needs to be shut off
        if (this.LevelTimer.IsRunning && Application.loadedLevel < 8 || Application.loadedLevel > 12) {
            // unload the level time and coordinate objects
            this.LevelTimer.Reset();
            this.LevelCoordinateManager = null;
            this.LevelCoordinates = null;
            this.index = 0;
            Application.LoadLevel(Application.loadedLevel);
        }

        // handle if the level coordinates are currently undefined
        if ((this.LevelCoordinates == null || this.LevelCoordinateManager == null) && Application.loadedLevel >= 8 && Application.loadedLevel <= 12) {
            // try to define the current level coordinates all the time
            this.LevelCoordinateManager = LevelCoordinateActivator.GetInstance(Application.loadedLevel);
            this.LevelCoordinates = this.LevelCoordinateManager.GetCoordinateList();
        // ignore when the user is already going through a level
        } else if (Application.loadedLevel >= 8 && Application.loadedLevel <= 12) {
            // check if the level is being reloaded
            if (Application.isLoadingLevel) {
                // if the level is being reset, reset everything
                this.LevelTimer.Reset();
                this.LevelCoordinateManager = null;
                this.LevelCoordinates = null;
                this.index = 0;
            }
        // otherwise, handle when the level coordinates are already define
        } else {
            // unload the level time and coordinate objects
            this.LevelTimer.Reset();
            this.LevelCoordinateManager = null;
            this.LevelCoordinates = null;
            this.index = 0;
        }

        // handle the updates when the list of coordinates is already defined
        if (this.LevelCoordinates != null) {
            // check if the counter is on a checking frame, return early if not right frame
            if (((this.counter += 1) % 30) != 0) return;
            // get the player's current position
            this.currentPosition = Android.Instance.gameObject.transform.rigidbody.position;
            // check whether the user is within range any future coordinates
            int collidedIndex = -1; for (int i = index; i < this.LevelCoordinates.Count; i ++) {
                // if not within range, continue as normal...
                if (!this.LevelCoordinates[i].CheckCollide(this.currentPosition)) continue;
                // otherwise set the collided index variable and break
                collidedIndex = i;
                index = collidedIndex;
                break;
            }
            // if a point was not reached, break out of this if statement
            if (collidedIndex == -1) return;
            // set the visited time of the current coordinate to the current elapsed time
            if (this.LevelCoordinates[index].Visited) return;
            this.LevelCoordinates[index].VisitedTime = this.LevelTimer.Elapsed;
            this.LevelCoordinates[index].Visited = true;
            // if within range, increment the index variable
            if (this.index == this.LevelCoordinates.Count - 1) {
                // turn off the timer if the index is at the very end
                this.LevelTimer.Stop();
                return;
            }
            // increment the index
            this.index = this.index + 1;
        }

    }

    private const int BOX_OFFSET = 20;
    private const int BOX_WIDTH = 200;
    private const int LABEL_WIDTH = 120;
    private const int LINE_HEIGHT = 20;
    // method to be called every time the gui is updated
    void OnGUI()
    {
        // first check that the set of level coordinates is not null
        if (this.LevelCoordinates == null) return;
        // check if the timer has been started yet.  if not, start it.
        if (!this.LevelTimer.IsRunning && this.index < this.LevelCoordinates.Count - 1 && !Application.isLoadingLevel) this.LevelTimer.Start();
        // construct the box that holds all of the labels in it
        int totalHeight = (this.LevelCoordinates.Count + 1) * LINE_HEIGHT;
        GUI.Box(new Rect(BOX_OFFSET, BOX_OFFSET, BOX_WIDTH, totalHeight), this.LevelTimer.Elapsed.ToString());
        // loop through all of the LevelCoordinate objects and display them in the GUI
        for (int i = 0; i < this.LevelCoordinates.Count; i ++) {
            // calculate the new height of the current label
            int labelHeight = (i + 1) * LINE_HEIGHT;
            // check if the current coordinate has already been visited or not
            String coordinateNameString = null;
            String coordinateTimeString = null;
            if (this.LevelCoordinates[i].Visited) {
                // construct a stringbuilder for the name of the current coordinate
                StringBuilder coordinateNameStringBuilder = new StringBuilder();
                // set the color as blue to indicate that it has already been visited
                coordinateNameStringBuilder.Append("<color=#0000ffff>");
                coordinateNameStringBuilder.Append(this.LevelCoordinates[i].Name);
                coordinateNameStringBuilder.Append("</color>");
                coordinateNameString = coordinateNameStringBuilder.ToString();
                // construct a stringbuilder for the time of the current coordinate
                StringBuilder coordinateTimeStringBuilder = new StringBuilder();
                // set the color as green for the time
                coordinateTimeStringBuilder.Append(this.LevelCoordinates[i].ElapsedTimeString());
                coordinateTimeString = coordinateTimeStringBuilder.ToString();
            // handle the coordinates that have not yet been visited
            } else {
                // construct a stringbuilder for the name of the current coordinate
                StringBuilder coordinateNameStringBuilder = new StringBuilder();
                // set the color as blue to indicate that it has already been visited
                coordinateNameStringBuilder.Append(this.LevelCoordinates[i].Name);
                coordinateNameString = coordinateNameStringBuilder.ToString();
                // construct a stringbuilder for the time of the current coordinate
                StringBuilder coordinateTimeStringBuilder = new StringBuilder();
                // handle the condition under which the checkpoint was skipped
                if (index > i) coordinateTimeStringBuilder.Append("<<SKIPPED>>");
                // handle the condition under which the checkpoint was not skipped
                else coordinateTimeStringBuilder.Append(this.LevelCoordinates[i].ElapsedTimeString());
                coordinateTimeString = coordinateTimeStringBuilder.ToString();
            }
            // write the labels onto the gui box
            GUI.Label(new Rect(BOX_OFFSET + 5, BOX_OFFSET + labelHeight, LABEL_WIDTH, LINE_HEIGHT), coordinateNameString);
            GUIStyle timeStyle = GUI.skin.GetStyle("Label"); timeStyle.alignment = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(BOX_OFFSET + LABEL_WIDTH, BOX_OFFSET + labelHeight, LABEL_WIDTH, LINE_HEIGHT), coordinateTimeString);
        }
    }

    //Log a message to the default log file
    private void LogMessage(String message, params System.Object[] args)
    {
        DCPMLogger.LogMessage("[Timer] " + message, args: args);
    }
}



