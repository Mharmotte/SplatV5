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

		var allSpawnPoints = Entity.All.OfType<SpawnPoint>();
		var randomSpawnPoints = allSpawnPoints.OrderBy( spawnPoint => spawnPoint.Position.Distance(Vector3.Zero) ).FirstOrDefault();
	}
}
