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
		Vector3 targetPos;
		var pos = Position + Vector3.Up * 40;
		var rot = Camera.Rotation * Rotation.FromAxis( Vector3.Up, -30 );

		float distance = 80.0f * Scale;
		targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 50) * Scale);
		targetPos += rot.Forward * -distance;

		var tr = Trace.Ray( pos, targetPos )
			.WithAnyTags( "solid" )
			.Ignore( this )
			.Radius( 8 )
			.Run();

		Camera.FirstPersonViewer = null;
		Camera.Position = tr.EndPosition;
	}
	public override void BuildInput()
	{
		base.BuildInput();
		InputDirection = Input.AnalogMove;
	}
}
