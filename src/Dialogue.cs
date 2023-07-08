using System.Collections.Generic;
using System;
using Godot;

public static class Dialogue {
    public static List<String> greetings = new List<String>{"Hello there", "I'm looking for powerful objects", "I've heard that you're the best seller for the budget", "Hi"};
}

public struct DialogeTexts {
    public DialogeTexts(String text, Personality personality, PlayerClass playerClass) {
        this.text = text;
        this.personality = personality;
        this.playerClass = playerClass;
    }
    public String text {get; set;}
    public Personality personality {get; set;}
    public PlayerClass? playerClass {get; set;}
}