using Godot;

public partial class Card : Node
{

	public static readonly Card[] cardsList = new Card[]
	{
		new AttributeBoostCard("Chance de Ataque Critico", "Aumenta as chances do seu Tank de dar um tremendo dano extra em 15%!", "CriticalChance", 15),
		new AttributeBoostCard("Dano", "Aumenta o dano total de seu poderoso tank em 10%. Uma poderosa arma contra brutamontes.", "Damage", 10),
		new AttributeBoostCard("Velocidade de ataque", "Aumenta a velocidade de ataque do seu Tank em 10%. Muito útil contra enormes hordas de inimigos mais fracos", "FireRate", 10),
		new AttributeBoostCard("Velocidade de movimento", "Aumenta sua velocidade de movimento em 15%. Ajuda a esquivar inimigos.", "Speed", 15),
		new ComponentCard("Cano duplo", "Esse é um atributo único! Da um cano duplo ao seu Tank, duplicando o seu dano total!", "res://Scenes/Components/DroneComponent.tscn"),
		new AttributeBoostCard("Drones", "Esse é um atributo único! Te da acesso a drones que automaticamente atacam os inimigos próximos.", "FireRate", 15),
	};

	public string CardName { get; set; }
	public string CardDescription { get; set; }

	public Card(string cardName, string cardDescription)
	{

		CardName = cardName;
		CardDescription = cardDescription;
	}

	public override void _Ready()
	{
	}
}
