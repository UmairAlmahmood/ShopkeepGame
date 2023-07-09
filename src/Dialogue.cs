using System.Collections.Generic;
using System;
using Godot;

public static class Dialogue {
    public static List<String> greetings = new List<String>{"Hello there", "I'm looking for powerful objects", "I've heard that you're the best seller for the budget", "Hi"};
    public static Dictionary<PlayerTypes, String[]> greetingsDict = new Dictionary<PlayerTypes, string[]>{ 
        [new PlayerTypes(Personality.Jolly, null)] = new String[]{"Hi There!", "Hello"},
        [new PlayerTypes(Personality.Cheapskate, null)] = new String[]{"I've heard that you're the best seller for the budget", "I've come in search of cheap but powerful items"},
        [new PlayerTypes(Personality.Foolhardy, null)] = new String[]{"I've come for items powerful enough to make me unmatched", "Hello there, do you house powerful objects?"},
        [new PlayerTypes(Personality.Cowardly, null)] = new String[]{"Do you have weapons that will keep me safe?", "Hi there, do you have reliable items?"},
    };
    
    public static String getGreeting(Personality playerPersonality, PlayerClass? playerClass = null) {
        Random random = new Random();
        PlayerTypes dialogeType = new PlayerTypes(playerPersonality, null);
        String[] playerGreetings = greetingsDict[dialogeType];
        String specificGreetings = playerGreetings[random.Next(0, playerGreetings.Length)];
        return specificGreetings;
    }
    public static String getDialogue(Personality playerPersonality, PlayerClass? playerClass) {
        return "";
    }
}

public struct PlayerTypes {
    public PlayerTypes(Personality personality, PlayerClass? playerClass) {
        this.personality = personality;
        this.playerClass = playerClass;
    }
    public Personality personality {get;set;}
    public PlayerClass? playerClass {get;set;}
}