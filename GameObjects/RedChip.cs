using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
class RedChip : Item{

    public RedChip(Texture2D texture) : base(texture)
    {
    }
    public override void OnBuy()
    {
        base.OnBuy();
    }
}