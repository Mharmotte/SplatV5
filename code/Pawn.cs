using Sandbox;
using System.ComponentModel;

namespace SplatoonV5;

public partial class Pawn : AnimatedEntity
{
	[ClientInput]
	public Vector3 InputDirection { get; set; }
	public float speed = 20f;
	public override void Spawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public void DressFromClient( IClient cl )
	{
		var c = new ClothingContainer();
		c.LoadFromClient( cl );
		c.DressEntity( this );
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

	public override void BuildInput()
	{
		base.BuildInput();
		InputDirection = Input.AnalogMove;
	}

	bool IsThirdPerson { get; set; } = false;

	public override void FrameSimulate( IClient cl )
	{

		Camera.Position = Position + Vector3.Backward * 10;
		Camera.Rotation = Rotation.FromPitch( 90f );
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs, float liftFeet = 0.0f )
	{
		if ( liftFeet > 0 )
		{
			start += Vector3.Up * liftFeet;
			maxs = maxs.WithZ( maxs.z - liftFeet );
		}

		var tr = Trace.Ray( start, end )
					.Size( mins, maxs )
					.WithAnyTags( "solid", "playerclip", "passbullets" )
					.Ignore( this )
					.Run();

		return tr;
	}
}
