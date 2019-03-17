using UnityEngine;

namespace RainbowLighting
{
	// Token: 0x02000003 RID: 3
	internal class ColorRandom : SimpleColorSO
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002143 File Offset: 0x00000343
		public override void SetColor(Color c)
		{
			this._color = c;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002150 File Offset: 0x00000350
		public override Color color
		{
			get
			{
				return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), Random.Range(.65f, .82f));
                //Added Alpha randomization because the lights are BIG BRIGHT in the new update
			}
		}
	}
}
