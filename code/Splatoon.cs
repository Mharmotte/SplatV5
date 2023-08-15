using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SplatoonV5;
public partial class SplatoonV5 : GameManager
{
	public SplatoonV5()
	{
		if ( Game.IsClient )
			new HUD();
	}
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new Pawn();
		client.Pawn = pawn;

		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 50.0f;
			pawn.Transform = tx;
		}
	}
}
