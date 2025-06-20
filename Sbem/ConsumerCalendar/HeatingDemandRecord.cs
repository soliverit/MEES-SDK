using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	public class HeatingDemandRecord : UsageRecordBase
	{
		public HeatingDemandRecord(int month, float internalGains, float solarGains, float solarGainsNoTrans, float solarControl, float solarControlDiffuse,
			float transmissionLosses, float ventilationLosses, float glazingRatio, float roomHeatCapacity,
			float totalGains, float aFactor, float heatTimeConstant, float heatReductionFactor, float spaceHeatingDemand)
			: base(month)
		{
			InternalGains = internalGains;
			SolarGains = solarGains;
			SolarGainsNoTransmission = solarGainsNoTrans;
			SolarControl = solarControl;
			SolarControlDiffuse = solarControlDiffuse;
			TransmissionLosses = transmissionLosses;
			VentilationInfiltrationLosses = ventilationLosses;
			GlazingRatio = glazingRatio;
			RoomHeatCapacity = roomHeatCapacity;
			TotalGains = totalGains;
			AFactor = aFactor;
			HeatTimeConstant = heatTimeConstant;
			HeatReductionFactor = heatReductionFactor;
			SpaceHeatingDemand = spaceHeatingDemand;
		}

		public HeatingDemandRecord(string month, float internalGains, float solarGains, float solarGainsNoTrans, float solarControl, float solarControlDiffuse,
			float transmissionLosses, float ventilationLosses, float glazingRatio, float roomHeatCapacity,
			float totalGains, float aFactor, float heatTimeConstant, float heatReductionFactor, float spaceHeatingDemand)
			: base(month)
		{
			InternalGains					= internalGains;
			SolarGains						= solarGains;
			SolarGainsNoTransmission		= solarGainsNoTrans;
			SolarControl					= solarControl;
			SolarControlDiffuse				= solarControlDiffuse;
			TransmissionLosses				= transmissionLosses;
			VentilationInfiltrationLosses	= ventilationLosses;
			GlazingRatio					= glazingRatio;
			RoomHeatCapacity				= roomHeatCapacity;
			TotalGains						= totalGains;
			AFactor							= aFactor;
			HeatTimeConstant				= heatTimeConstant;
			HeatReductionFactor				= heatReductionFactor;
			SpaceHeatingDemand				= spaceHeatingDemand;
		}

		public static HeatingDemandRecord FromLine(string line)
		{
			string[] v = line.Split(',');

			return new HeatingDemandRecord(
				v[0],
				float.Parse(v[1]),   // Qi
				float.Parse(v[2]),   // Qsun
				float.Parse(v[3]),   // Qsun;nt
				float.Parse(v[4]),   // Qsc
				float.Parse(v[5]),   // Qscd
				float.Parse(v[6]),   // Qtr
				float.Parse(v[7]),   // Qvent+inf
				float.Parse(v[8]),   // glratio
				float.Parse(v[9]),   // Rb;heat
				float.Parse(v[10]),  // Qgain
				float.Parse(v[11]),  // a-factor
				float.Parse(v[12]),  // Tau-heat
				float.Parse(v[13]),  // a-heat;red
				float.Parse(v[14])   // Qdem;heat;m;room
			);
		}

		public float InternalGains { get; protected set; }                // Qi
		public float SolarGains { get; protected set; }                   // Qsun
		public float SolarGainsNoTransmission { get; protected set; }    // Qsun;nt
		public float SolarControl { get; protected set; }                 // Qsc
		public float SolarControlDiffuse { get; protected set; }         // Qscd
		public float TransmissionLosses { get; protected set; }          // Qtr
		public float VentilationInfiltrationLosses { get; protected set; } // Qvent+inf
		public float GlazingRatio { get; protected set; }                // glratio
		public float RoomHeatCapacity { get; protected set; }            // Rb;heat
		public float TotalGains { get; protected set; }                  // Qgain
		public float AFactor { get; protected set; }                     // a-factor
		public float HeatTimeConstant { get; protected set; }            // Tau-heat
		public float HeatReductionFactor { get; protected set; }         // a-heat;red
		public float SpaceHeatingDemand { get; protected set; }          // Qdem;heat;m;room

		public void Add(HeatingDemandRecord record)
		{
			InternalGains					+= record.InternalGains;
			SolarGains						+= record.SolarGains;
			SolarGainsNoTransmission		+= record.SolarGainsNoTransmission;
			SolarControl					+= record.SolarControl;
			SolarControlDiffuse				+= record.SolarControlDiffuse;
			TransmissionLosses				+= record.TransmissionLosses;
			VentilationInfiltrationLosses	+= record.VentilationInfiltrationLosses;
			GlazingRatio					+= record.GlazingRatio;
			RoomHeatCapacity				+= record.RoomHeatCapacity;
			TotalGains						+= record.TotalGains;
			AFactor							+= record.AFactor;
			HeatTimeConstant				+= record.HeatTimeConstant;
			HeatReductionFactor				+= record.HeatReductionFactor;
			SpaceHeatingDemand				+= record.SpaceHeatingDemand;
		}
		public void Subtract(HeatingDemandRecord record)
		{
			InternalGains					-= record.InternalGains;
			SolarGains						-= record.SolarGains;
			SolarGainsNoTransmission		-= record.SolarGainsNoTransmission;
			SolarControl					-= record.SolarControl;
			SolarControlDiffuse				-= record.SolarControlDiffuse;
			TransmissionLosses				-= record.TransmissionLosses;
			VentilationInfiltrationLosses	-= record.VentilationInfiltrationLosses;
			GlazingRatio					-= record.GlazingRatio;
			RoomHeatCapacity				-= record.RoomHeatCapacity;
			TotalGains						-= record.TotalGains;
			AFactor							-= record.AFactor;
			HeatTimeConstant				-= record.HeatTimeConstant;
			HeatReductionFactor				-= record.HeatReductionFactor;
			SpaceHeatingDemand				-= record.SpaceHeatingDemand;
		}
		public void MultiplyBy(float factor)
		{
			InternalGains					*= factor;
			SolarGains						*= factor;
			SolarGainsNoTransmission		*= factor;
			SolarControl					*= factor;
			SolarControlDiffuse				*= factor;
			TransmissionLosses				*= factor;
			VentilationInfiltrationLosses	*= factor;
			GlazingRatio					*= factor;
			RoomHeatCapacity				*= factor;
			TotalGains						*= factor;
			AFactor							*= factor;
			HeatTimeConstant				*= factor;
			HeatReductionFactor				*= factor;
			SpaceHeatingDemand				*= factor;
		}

		public HeatingDemandRecord Clone()
		{
			return new HeatingDemandRecord(Month, InternalGains, SolarGains, SolarGainsNoTransmission, SolarControl,
				SolarControlDiffuse, TransmissionLosses, VentilationInfiltrationLosses, GlazingRatio, RoomHeatCapacity,
				TotalGains, AFactor, HeatTimeConstant, HeatReductionFactor, SpaceHeatingDemand);
		}
	}

}
