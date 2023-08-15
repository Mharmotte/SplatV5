using Sandbox;
using System.ComponentModel;

namespace SplatoonV5;

public partial class Pawn : AnimatedEntity
{
	[ClientInput] public Vector3 InputDirection { get; set; }
	public float speed = 10f;
	public override void Spawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}


	public override void Simulate( IClient cl )
	{
		var moveDirection = InputDirection.Normal;

		Velocity = moveDirection * speed;
		Position += Velocity + Time.Delta;

		if ( Game.IsServer && Input.Pressed( "attack1" ) )
		{
			var Peinture = new ModelEntity();
			Peinture.SetModel( "models/citizen/citizen.vmdl" );
			Peinture.Position = Position + Rotation.Forward * 40;
			Peinture.Rotation = Rotation.LookAt( Vector3.Random.Normal );
			Peinture.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			Peinture.PhysicsGroup.Velocity = Rotation.Forward * 1000;
		}
	}
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );
		Camera.Position = Position + Vector3.Backward * 100;
	}
	public override void BuildInput()
	{
		base.BuildInput();
		InputDirection = Input.AnalogMove;
	}
}
