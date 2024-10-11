using Godot;

public partial class ComponentCard : Card
{

	public PackedScene ComponentScene { get; set; }

	public ComponentCard(string cardName, string cardDescription, string scenePath) : base(cardName, cardDescription)
	{

		CardName = cardName;
		CardDescription = cardDescription;
		ComponentScene = GD.Load(scenePath) as PackedScene;
	}
}
