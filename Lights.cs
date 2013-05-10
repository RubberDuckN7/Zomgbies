using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LudumDareGame
{
    class Lights
    {
        Vector2 start,position, destination;
     
        public Lights(Vector2 spos, Vector2 dest) {

            start = spos;
            destination = dest;
     
            position = start;
        }

        public void Update() {

            Vector2 dir = destination - start;
            Vector2 len = destination - position;

            if (len.Length() <= 0.1f) {
                
                Vector2 tmp = destination;
                destination = start;
                start = tmp;
                dir = destination - start;
            }

            dir.Normalize();

            position.X += 0.001f * dir.X;
            position.Y += 0.001f * dir.Y;

        }

        public Vector2 getPos() {
            return position;
        }
    }
}
