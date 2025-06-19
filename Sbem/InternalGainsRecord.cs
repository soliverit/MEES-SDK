using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem
{
	public class InternalGainsRecord : UsageRecordBase
	{
		public InternalGainsRecord(int month, float people, float appliances, float lightingInternal, float ventilation, float total, float lightingPowerDensity, float daylightingPercent, float lightingEnergyFactor, float wallFraction)
			: base(month)
		{
			People = people;
			Appliances = appliances;
			LightingInternal = lightingInternal;
			Ventilation = ventilation;
			TotalGains = total;
			LightingPowerDensity = lightingPowerDensity;
			DaylightingPercent = daylightingPercent;
			LightingEnergyFactor = lightingEnergyFactor;
			WallFraction = wallFraction;
		}

		public InternalGainsRecord(string month, float people, float appliances, float lightingInternal, float ventilation, float total, float lightingPowerDensity, float daylightingPercent, float lightingEnergyFactor, float wallFraction)
			: base(month)
		{
			People = people;
			Appliances = appliances;
			LightingInternal = lightingInternal;
			Ventilation = ventilation;
			TotalGains = total;
			LightingPowerDensity = lightingPowerDensity;
			DaylightingPercent = daylightingPercent;
			LightingEnergyFactor = lightingEnergyFactor;
			WallFraction = wallFraction;
		}

		public static InternalGainsRecord FromLine(string line)
		{

			string[] values		= line.Split(',');
			float[] floatValues = new float[line.Length - 1];
			// Try to parse the optional parameters
			float placeHodler = 0;
			for(int valueID = 1; valueID < values.Length; valueID++)
				floatValues[valueID - 1]	= values.Length > valueID && float.TryParse(values[valueID], out placeHodler) ? placeHodler : 0;
			return new InternalGainsRecord(
				values[0],
				floatValues[0],
				floatValues[2],
				floatValues[3],
				floatValues[4],
				floatValues[5],
				floatValues[6],
				floatValues[7],
				floatValues[8],
				floatValues[9]
			);
		}

		public float People { get; protected set; }                // Qi;pers
		public float Appliances { get; protected set; }            // Qi;app
		public float LightingInternal { get; protected set; }      // Qi;li
		public float Ventilation { get; protected set; }           // Qi;vent
		public float TotalGains { get; protected set; }            // Qi (total)
		public float LightingPowerDensity { get; protected set; }  // Light Power Dens
		public float DaylightingPercent { get; protected set; }    // DL%
		public float LightingEnergyFactor { get; protected set; }  // LFE
		public float WallFraction { get; protected set; }          // W/F

		public void Add(InternalGainsRecord record)
		{
			People					+= record.People;
			Appliances				+= record.Appliances;
			LightingInternal		+= record.LightingInternal;
			Ventilation				+= record.Ventilation;
			TotalGains				+= record.TotalGains;
			LightingPowerDensity	+= record.LightingPowerDensity;
			DaylightingPercent		+= record.DaylightingPercent;
			LightingEnergyFactor	+= record.LightingEnergyFactor;
			WallFraction			+= record.WallFraction;
		}
		public void Subtract(InternalGainsRecord record)
		{
			People					-= record.People;
			Appliances				-= record.Appliances;
			LightingInternal		-= record.LightingInternal;
			Ventilation				-= record.Ventilation;
			TotalGains				-= record.TotalGains;
			LightingPowerDensity	-= record.LightingPowerDensity;
			DaylightingPercent		-= record.DaylightingPercent;
			LightingEnergyFactor	-= record.LightingEnergyFactor;
			WallFraction			-= record.WallFraction;
		}
		public void MultiplyBy(float factor)
		{
			People					*= factor;
			Appliances				*= factor;
			LightingInternal		*= factor;
			Ventilation				*= factor;
			TotalGains				*= factor;
			LightingPowerDensity	*= factor;
			DaylightingPercent		*= factor;
			LightingEnergyFactor	*= factor;
			WallFraction			*= factor;
		}

		public InternalGainsRecord Clone()
		{
			return new InternalGainsRecord(Month, People, Appliances, LightingInternal, Ventilation, TotalGains, LightingPowerDensity, DaylightingPercent, LightingEnergyFactor, WallFraction);
		}
	}

}
