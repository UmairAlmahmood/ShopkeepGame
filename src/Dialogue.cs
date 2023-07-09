using System.Collections.Generic;
using System;
using Godot;

public static class Dialogue {
    public static List<String> greetings = new List<String>{"Hello there", "I'm looking for powerful objects", "I've heard that you're the best seller for the budget", "Hi"};
    public static Dictionary<PlayerTypes, String> greetingsDict = new Dictionary<PlayerTypes, String>{ 
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.VsUndead)] = "Cloudy skies and mists rolling through the forest. Looks like a perfect day to purge some zombies! Hahaha. Do you have anything that may aid me in my endeavors?",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.VsGoblins)] = "Good news! I stumbled upon the camp of the goblins that have been raiding the town. I was wondering if you had any tools that I could use to put an end to their pillaging.",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.Dungeoneer)] = "Hello there! I’ve come to this peaceful town to recuperate after delving into several dungeons. I thought I’d see if you carried any gear I could use in the field.",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.Diver)] = "Salutations, friend! It’s good to be back on land for a change! Alas, I’ll be back to fighting those damn mer-raiders soon enough, as per the King’s orders. Say, any chance you’ve got something I might find of use?",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Warrior, SpecialTrait.CurseHunter)] = "Greetings honest shopkeep! I’ve heard rumors that you carry cursed items. Do you have any in stock?",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Archer, SpecialTrait.VsUndead)] = "Cloudy skies and mists rolling through the forest. Looks like a perfect day to purge some zombies! Hahaha. Do you have anything that may aid me in my endeavors?",
        [new PlayerTypes(Personality.Jolly, PlayerClass.Mage, SpecialTrait.VsUndead)] = "Cloudy skies and mists rolling through the forest. Looks like a perfect day to purge some zombies! Hahaha. Do you have anything that may aid me in my endeavors?",
    };
    
    public static String getGreeting(Personality playerPersonality, PlayerClass playerClass, SpecialTrait specialTrait) {
        try {
            PlayerTypes dialogeType = new PlayerTypes(playerPersonality, playerClass, specialTrait);
            String playerGreetings = greetingsDict[dialogeType];
            return playerGreetings;
        } catch {
            return "AAA";
        }
    }
    public static String getDialogueFromWillingess(float willingness) {
        string response = "";
        if(willingness < .1) {
            response = "Is this a joke?";
        } if(willingness < .3) {
            response = "I'm not impressed";
        } else if(willingness < .5) {
            response = "I see the value but it isn't for me";
        } else if(willingness < .7) {
            response = "I am intriguied by the item, tell me more";
        } else if(willingness < .95) {
            response = "I do like this item very much, it does seem useful to me";
        } else if (willingness < 1.0) {
            response = "Let it already be sold";
        } else {
            response = "...\nI will literally die for this";
        }
        GD.Print(willingness);
        return response;
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