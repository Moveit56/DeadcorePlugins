﻿// namespace for the level coordinates
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// namespace for hiding 
namespace LevelCoordinates { 

    // interface to define all method for the level coordinates
    public interface LevelCoordinateIF {
        // method to get the list of checkpoint coordinates
        List<LevelCoordinate> GetCoordinateList();
    }

    // abstract class to define common behaviors and non-public components
    internal abstract class AbstractLevelCoordinate : LevelCoordinateIF {
        // private field for accessing the coordinates - defined in implementing constructors
        protected List<LevelCoordinate> coordinateList;
        // implement the getCoordinateList method
        public List<LevelCoordinate> GetCoordinateList() {
            // return the coordinate list
            return this.coordinateList;
        }
        // make the ToString method abstract
        public abstract override string ToString();
    }

    // class to act as a proxy for the level coordinate objects
    public class LevelCoordinateActivator : LevelCoordinateIF {
        // private field for the instance of this class
        private static LevelCoordinateActivator Instance = null;
        // private field for the level coordinate object
        private AbstractLevelCoordinate LevelCoordinateObject;
        // make the constructor private so as to assert the singleton design pattern
        private LevelCoordinateActivator() { }
        // method to get an instance of the 
        public static LevelCoordinateIF GetInstance(int levelID) {
            // check if the instance of the LevelCoordinateActivator is null
            if (Instance == null) Instance = new LevelCoordinateActivator();
            // construct the type name
            StringBuilder typeBuilder = new StringBuilder();
            typeBuilder.Append("LevelCoordinates.Level").Append(Hash(levelID)).Append("Coordinates");
            // building the instance of the actual coordinate object
            // attempt to get a concrete type of the target class
            Type type = null;
            try { type = Type.GetType(typeBuilder.ToString());
            } catch (Exception /* ex */) { /* DCPMCommon.DCPMLogger.LogMessage(ex.Message); */ }
            // attempt to get a concrete object of the target class
            try { Instance.LevelCoordinateObject = (AbstractLevelCoordinate) Activator.CreateInstance(type);
            } catch (Exception /* ex */) { /* DCPMCommon.DCPMLogger.LogMessage(ex.Message); */ }
            // return the level activator object
            return Instance;
        }
        // delegate the task to the private level coordinate object
        public List<LevelCoordinate> GetCoordinateList() {
            // return the coordinate list of the instantiated object
            if (this.LevelCoordinateObject == null) { return null; }
            return this.LevelCoordinateObject.GetCoordinateList();
        }
        // method to convert the level ID into the level number
        private static int Hash(int levelID) {
            // iterate through the level values
            // DCPMCommon.DCPMLogger.LogMessage("Processing hash code "+levelID+"\n");
            switch (levelID) {
                case  8: return 1; // level 1's ID is 7
                case  9: return 2; // level 2's ID is 9
                case 10: return 3; // level 3's ID is 10
                case 11: return 4; // level 4's ID is 11
                case 12: return 5; // level 5's ID is 12
                default: return 0; // any other values can return zero
            }
        }
    }

    // class for level 1 (level ID #8)
    internal class Level1Coordinates : AbstractLevelCoordinate {
        // override the default level coordinate list implementation
        public Level1Coordinates() {
            // create the list of coordinates
            this.coordinateList = new List<LevelCoordinate>() {
                new LevelCoordinate("The First Hop  ", new Vector3(-928f, -527f,    1f)), // THE FIRST HOP
                new LevelCoordinate("Leave The Spawn", new Vector3(-894f, -486f,    1f)), // OUT OF THE SPAWN
                new LevelCoordinate("Around The Wall", new Vector3(-682f, -390f,    1f)), // AROUND THE WALL
                new LevelCoordinate("Tower Approach ", new Vector3(-477f, -355f,    1f)), // TOWER APPROACHES
                new LevelCoordinate("Zig Zags       ", new Vector3(-363f, -321f,   48f)), // ZIG ZAGS
                new LevelCoordinate("Box Hops       ", new Vector3(-328f, -313f,  116f)), // BOX HOPPING
                new LevelCoordinate("More Box Hops  ", new Vector3(-221f, -301f,  113f)), // EVEN MORE BOX HOPPING
                new LevelCoordinate("Turret Dash    ", new Vector3(-100f, -288f,  113f)), // DASH PAST THE TURRETS
                new LevelCoordinate("Up And Around  ", new Vector3( 113f, -245f,   98f)), // UP AND AROUND
                new LevelCoordinate("Other Side     ", new Vector3(  80f, -190f,   -5f)), // TO THE OTHER SIDE
                new LevelCoordinate("Tumblers 1     ", new Vector3(  84f, -191f, -123f)), // TUMBLERS 1
                new LevelCoordinate("Tumblers 2     ", new Vector3( -76f, -131f, -136f)), // TUMBLERS 2
                new LevelCoordinate("Tumblers 3     ", new Vector3(-161f, -131f, -209f)), // TUMBLERS 3
                new LevelCoordinate("Fans!          ", new Vector3(-247f, -125f, -124f)), // COMMON ENEMY: FANS
                new LevelCoordinate("Finish Dash 1  ", new Vector3(-241f,  -95f,   -5f)), // DASH FOR THE FINISH 1
                new LevelCoordinate("Finish Dash 2  ", new Vector3(-335f,  -82f,    1f)), // DASH FOR THE FINISH 2
                new LevelCoordinate("Ending         ", new Vector3(-238f,   19f,   -6f))  // ENDING
            };
        }
        // override the ToString method to return the level description
        public override string ToString() {
            // return what level this is
            return "Level 1 (Level ID 8)";
        }
    }

    // class for level 2 (Level ID #9)
    internal class Level2Coordinates : AbstractLevelCoordinate {
        // override the default level coordinate list implementation
        public Level2Coordinates() {
            // create the list of coordinates
            this.coordinateList = new List<LevelCoordinate>() {
                new LevelCoordinate("Energy Shields ", new Vector3(-239f,   49f,  -53f)),
                new LevelCoordinate("Shield Wheel   ", new Vector3(-219f,   70f,  -37f)),
                new LevelCoordinate("Post-Wheel     ", new Vector3(-174f,   97f,    0f)),
                new LevelCoordinate("Trapped!       ", new Vector3(-219f,  167f,  -16f)),
                new LevelCoordinate("Around the Edge", new Vector3(-236f,  215f,  -19f)),
                new LevelCoordinate("Choose a Path  ", new Vector3(-150f,  216f,    0f)),
                new LevelCoordinate("Boxes (Right)  ", new Vector3(-158f,  264f,  -55f)),
                new LevelCoordinate("More Boxes!    ", new Vector3(-158f,  278f, -177f)),
                new LevelCoordinate("Crusher        ", new Vector3(  -3f,  302f, -206f)),
                new LevelCoordinate("Keyhole (Left) ", new Vector3(-150f,  289f,   39f)),
                new LevelCoordinate("Vertical Gates ", new Vector3( -99f,  321f,   75f)),
                new LevelCoordinate("Grids          ", new Vector3(  64f,  304f,  140f)),
                new LevelCoordinate("The Elevator   ", new Vector3( 149f,  332f,  114f)),
                new LevelCoordinate("Path Merge     ", new Vector3(  97f,  371f,    0f)),
                new LevelCoordinate("Pillars        ", new Vector3( 316f,  379f,    0f)),
                new LevelCoordinate("Choose a Path 2", new Vector3( 385f,  422f,   -6f)),
                new LevelCoordinate("Mind Your Head ", new Vector3( 465f,  421f,  -12f)),
                new LevelCoordinate("Ending         ", new Vector3( 186f,  665f,    0f))
            };
        }
        public override string ToString() {
            return "Level 2 (Level ID 9)";
        }
    }

    // class for all of the coordinates
    public class LevelCoordinate {
        // private field for the coordinates
        public Vector3 Location { get; private set; }
        // private field for the name of the coordinate
        public String Name { get; private set; }
        // private boolean indicator for whether or not the coordinate has been visited
        public Boolean Visited { get; set; }
        // private variable for the time that they were visited at
        public TimeSpan VisitedTime { get; set; }
        // constructor ( name + coordinate )
        public LevelCoordinate(String name, Vector3 location) {
            // set the name of the coordinate
            this.Name = name;
            // set the location of the coordinate
            this.Location = location;
            // set the default state of visited to false
            this.Visited = false;
            // set the default state of visited time to null
            this.VisitedTime = new TimeSpan(0);
        }
        // public method for checking if the user is within the region of the coordinate
        private const float COLLISION_DISTANCE = 7f;
        public Boolean CheckCollide(Vector3 position) {
            // check the x coordinate, return false if out of range
            if (Math.Abs(this.Location.x - position.x) > COLLISION_DISTANCE) return false;
            // check the y coordinate, return false if out of range
            if (Math.Abs(this.Location.y - position.y) > COLLISION_DISTANCE) return false;
            // check the z coordinate, return false if out of range
            if (Math.Abs(this.Location.z - position.z) > COLLISION_DISTANCE) return false;
            // if all of the coordinates are within range, return true
            return true;
        }
        // method to output the elapsed time (more flexible than TimeSpan.ToString())
        public String ElapsedTimeString() {
            // check if the elapsed time is zero
            if (this.VisitedTime.Ticks == 0) { return "--:--:--.---"; }
            // construct a stringbuilder for the elapsed time of the coordinate
            StringBuilder stringBuilder = new StringBuilder();
            // handle the number of hours elapsed
            int hours = (int) Math.Floor(this.VisitedTime.TotalHours) % 24;
            if (Math.Floor(this.VisitedTime.TotalHours) > 0)
                stringBuilder.AppendFormat("{0:00}:", hours);
            // handle the number of minutes elapsed
            int minutes = (int) Math.Floor(this.VisitedTime.TotalMinutes) % 60;
            if (Math.Floor(this.VisitedTime.TotalMinutes) > 0)
                stringBuilder.AppendFormat("{0:00}:", minutes);
            // handle the number of seconds elapsed
            int seconds = (int) Math.Floor(this.VisitedTime.TotalSeconds) % 60;
            if (Math.Floor(this.VisitedTime.TotalSeconds) > 0)
                stringBuilder.AppendFormat("{0:00}.", seconds);
            // handle the number of milliseconds elapsed
            int millis = (int) Math.Floor(this.VisitedTime.TotalMilliseconds) % 1000;
            if (Math.Floor(this.VisitedTime.TotalMilliseconds) > 0)
                stringBuilder.AppendFormat("{0:000}", millis);
            // return the time string
            return stringBuilder.ToString();
        }
        // override the ToString method
        public override string ToString() {
            // make a string builder for formatting the string
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.Name);
            // stringBuilder.Append(" (");
            // stringBuilder.Append(Math.Round(this.Location.x)).Append(", ");
            // stringBuilder.Append(Math.Round(this.Location.y)).Append(", ");
            // stringBuilder.Append(Math.Round(this.Location.z)).Append(")");
            // return the constructed string
            return stringBuilder.ToString();
        }
    }

}