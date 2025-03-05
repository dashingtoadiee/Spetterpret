using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuskModules.DynamicVariables {

	/// <summary> Color setting to act as a target color for graphics </summary>
	[System.Serializable]
	public class ColorSetting {

		/// <summary> What part of the color is used </summary>
		public enum ColorUse {
			full,
			nothing,
			colorOnly,
			alphaOnly
		}

		[Tooltip("Color of this setting")]
		public ColorReference color;
		[Tooltip("What part of the color is used")]
		public ColorUse colorUse;

		/// <summary> Current fill value for this color. </summary>
		public float fill { get; set; }

		/// <summary> When used for first time </summary>
		public ColorSetting() {
			if (color == null) color = new ColorReference();
			fill = 0;
		}
		/// <summary> When used for first time </summary>
		public ColorSetting(ColorSetting copy) {
			color = copy.color;
			colorUse = copy.colorUse;
		}

		/// <summary> Static method to combine multiple colors together. </summary>
		public static Color Combine(params ColorSetting[] colors) {
			return Combine(Color.white, colors);
		}
		
		/// <summary> Static method to combine multiple colors together. </summary>
		public static Color Combine(Color baseColor, params ColorSetting[] colors) {
			float a = 0;
			Color addColor = Color.black.WithA(0);
			float maxColorFill = 0;
			float maxAlphaFill = 0;
			
			for (int i = 0; i < colors.Length; i++) {
				if (colors[i].colorUse == ColorUse.full || colors[i].colorUse == ColorUse.colorOnly) {
					addColor += colors[i].color.value * colors[i].fill;
					if (colors[i].fill > maxColorFill) maxColorFill = colors[i].fill;
				}
				if (colors[i].colorUse == ColorUse.full || colors[i].colorUse == ColorUse.alphaOnly) {
					a += colors[i].color.value.a * colors[i].fill;
					if (colors[i].fill > maxAlphaFill) maxAlphaFill = colors[i].fill;
				}
			}
			addColor += baseColor * (1 - maxColorFill);
			a += baseColor.a * (1 - maxAlphaFill);
			return addColor.WithA(a);
		}
	}
	
}