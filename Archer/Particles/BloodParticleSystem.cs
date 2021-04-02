﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Archer.Particles
{
    public class BloodParticleSystem : ParticleSystem
    {
        Rectangle _source;
        public bool IsRaining { get; set; } = true;

        public BloodParticleSystem(Game game, Rectangle source) : base(game, 1)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "drop";
            minNumParticles = 1;
            maxNumParticles = 20;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 10, Vector2.UnitY * 100, Color.DarkRed, scale: 3f, lifetime: 3);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(IsRaining) AddParticles(_source);
        }

        public void CallLoadContent()
        {
            base.LoadContent();
        }
    }
}
