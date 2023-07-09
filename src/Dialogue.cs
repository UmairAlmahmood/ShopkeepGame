using System.Collections.Generic;
using System;
using Godot;

public static class Dialogue {
    public static List<String> greetings = new List<String>{"Hello there", "I'm looking for powerful objects", "I've heard that you're the best seller for the budget", "Hi"};
    public static Dictionary<PlayerTypes, String> greetingsDict = new Dictionary<PlayerTypes, String>{ 
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.VsUndead)] = "Cloudy skies and mists rolling through the forest. Looks like a perfect day to purge some zombies! Hahaha. Do you have anything that may aid me in my endeavors?",
    };
    
    public static String getGreeting(Personality playerPersonality = Personality.Jolly, PlayerClass playerClass = PlayerClass.Warrior, SpecialTrait specialTrait = SpecialTrait.VsUndead) {
        Random random = new Random();
        PlayerTypes dialogeType = new PlayerTypes(playerPersonality, playerClass, specialTrait);
        String playerGreetings = greetingsDict[dialogeType];
        return playerGreetings;
    }
    public static String getDialogue(Personality playerPersonality, PlayerClass? playerClass) {
        return "";
    }
}

public struct PlayerTypes {
    public PlayerTypes(Personality personality, PlayerClass playerClass, SpecialTrait specialTrait) {
        this.personality = personality;
        this.playerClass = playerClass;
        this.specialTrait = specialTrait;
    }
    public Personality personality {get;set;}
    public PlayerClass playerClass {get;set;}
    public SpecialTrait specialTrait {get;set;}
}