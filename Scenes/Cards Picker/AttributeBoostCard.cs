using Godot;
using System;

public partial class AttributeBoostCard : Card
{

    [Export]
    public float BoostPercentage
    {
        get => _boostPercentage;
        set => _boostPercentage = value is >= 0 and <= 100 ? value : throw new ArgumentException($"{nameof(BoostPercentage)} must be at least 0 and at most 100.");
    }
    [Export] public string AttributeName;

    private float _boostPercentage = 10f;

    public AttributeBoostCard(string cardName, string cardDescription, string attributeName, float boostPercentage) : base(cardName, cardDescription)
    {

        CardName = cardName;
        CardDescription = cardDescription;

        AttributeName = attributeName;
        BoostPercentage = boostPercentage;
    }
}