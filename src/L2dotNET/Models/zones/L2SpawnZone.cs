﻿using System.Collections.Generic;
using L2dotNET.Utility.Geometry;

namespace L2dotNET.Models.Zones
{
    public class L2SpawnZone : L2ZoneType
    {
        private List<Location> _spawnLocs;
        private List<Location> _chaoticSpawnLocs;

        public L2SpawnZone(int id) : base(id) { }

        public void AddSpawn(int x, int y, int z)
        {
            if (_spawnLocs == null)
                _spawnLocs = new List<Location>();

            _spawnLocs.Add(new Location(x, y, z));
        }

        public void AddChaoticSpawn(int x, int y, int z)
        {
            if (_chaoticSpawnLocs == null)
                _chaoticSpawnLocs = new List<Location>();

            _chaoticSpawnLocs.Add(new Location(x, y, z));
        }

        public List<Location> GetSpawns()
        {
            return _spawnLocs;
        }

        public Location GetSpawnLoc()
        {
            return (Location)Rnd.Get(_spawnLocs);
        }

        public Location GetChaoticSpawnLoc()
        {
            if (_chaoticSpawnLocs != null)
                return (Location)Rnd.Get(_chaoticSpawnLocs);

            return GetSpawnLoc();
        }

        public override void SetParameter(string name, string value) { }

        public override void OnDieInside(L2Character character) { }

        public override void OnReviveInside(L2Character character) { }

        protected override void OnEnter(L2Character character) { }

        protected override void OnExit(L2Character character) { }
    }
}