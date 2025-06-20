using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesSDK.Sbem.ConsumerCalendar
{
	public class CoolingDemandRecord : UsageRecordBase
	{
		public CoolingDemandRecord(int month, float internalGains, float solarGainsTrans, float solarGainsNoTrans,
			float solarControl, float transmissionLosses, float ventilationLosses, float ventilationNoCooling,
			float roomCoolCapacity, float totalLosses, float glazingRatio,
			float aFactor, float coolTimeConstant, float coolingReductionFactor, float spaceCoolingDemand)
			: base(month)
		{
			InternalGains				= internalGains;
			SolarGainsTransmitted		= solarGainsTrans;
			SolarGainsNoTransmission	= solarGainsNoTrans;
			SolarControl				= solarControl;
			TransmissionLosses			= transmissionLosses;
			VentilationLosses			= ventilationLosses;
			VentilationNoCooling		= ventilationNoCooling;
			RoomCoolCapacity			= roomCoolCapacity;
			TotalLosses					= totalLosses;
			GlazingRatio				= glazingRatio;
			AFactor						= aFactor;
			CoolTimeConstant			= coolTimeConstant;
			CoolingReductionFactor		= coolingReductionFactor;
			SpaceCoolingDemand			= spaceCoolingDemand;
		}

		public CoolingDemandRecord(string month, float internalGains, float solarGainsTrans, float solarGainsNoTrans,
			float solarControl, float transmissionLosses, float ventilationLosses, float ventilationNoCooling,
			float roomCoolCapacity, float totalLosses, float glazingRatio,
			float aFactor, float coolTimeConstant, float coolingReductionFactor, float spaceCoolingDemand)
			: base(month)
		{
			InternalGains				= internalGains;
			SolarGainsTransmitted		= solarGainsTrans;
			SolarGainsNoTransmission	= solarGainsNoTrans;
			SolarControl				= solarControl;
			TransmissionLosses			= transmissionLosses;
			VentilationLosses			= ventilationLosses;
			VentilationNoCooling		= ventilationNoCooling;
			RoomCoolCapacity			= roomCoolCapacity;
			TotalLosses					= totalLosses;
			GlazingRatio				= glazingRatio;
			AFactor						= aFactor;
			CoolTimeConstant			= coolTimeConstant;
			CoolingReductionFactor		= coolingReductionFactor;
			SpaceCoolingDemand			= spaceCoolingDemand;
		}

		public static CoolingDemandRecord FromLine(string line)
		{
			string[] v = line.Split(',');

			return new CoolingDemandRecord(
				v[0],
				float.Parse(v[1]),  // Qi
				float.Parse(v[2]),  // Qsun;t
				float.Parse(v[3]),  // Qsun;nt
				float.Parse(v[4]),  // Qsc
				float.Parse(v[5]),  // Qtr
				float.Parse(v[6]),  // Qvent+inf
				float.Parse(v[7]),  // QventNC
				float.Parse(v[8]),  // Rb;cool
				float.Parse(v[9]),  // Qloss
				float.Parse(v[10]), // glratio
				float.Parse(v[11]), // a-factor
				float.Parse(v[12]), // Tau-cool
				float.Parse(v[13]), // a-cool;red
				float.Parse(v[14])  // Qdem;cool;m;room
			);
		}

		public float InternalGains { get; protected set; }               // Qi
		public float SolarGainsTransmitted { get; protected set; }       // Qsun;t
		public float SolarGainsNoTransmission { get; protected set; }    // Qsun;nt
		public float SolarControl { get; protected set; }                // Qsc
		public float TransmissionLosses { get; protected set; }          // Qtr
		public float VentilationLosses { get; protected set; }           // Qvent+inf
		public float VentilationNoCooling { get; protected set; }        // QventNC
		public float RoomCoolCapacity { get; protected set; }            // Rb;cool
		public float TotalLosses { get; protected set; }                 // Qloss
		public float GlazingRatio { get; protected set; }                // glratio
		public float AFactor { get; protected set; }                     // a-factor
		public float CoolTimeConstant { get; protected set; }            // Tau-cool
		public float CoolingReductionFactor { get; protected set; }      // a-cool;red
		public float SpaceCoolingDemand { get; protected set; }          // Qdem;cool;m;room

		public void Add(CoolingDemandRecord record)
		{
			InternalGains				+= record.InternalGains;
			SolarGainsTransmitted		+= record.SolarGainsTransmitted;
			SolarGainsNoTransmission	+= record.SolarGainsNoTransmission;
			SolarControl				+= record.SolarControl;
			TransmissionLosses			+= record.TransmissionLosses;
			VentilationLosses			+= record.VentilationLosses;
			VentilationNoCooling		+= record.VentilationNoCooling;
			RoomCoolCapacity			+= record.RoomCoolCapacity;
			TotalLosses					+= record.TotalLosses;
			GlazingRatio				+= record.GlazingRatio;
			AFactor						+= record.AFactor;
			CoolTimeConstant			+= record.CoolTimeConstant;
			CoolingReductionFactor		+= record.CoolingReductionFactor;
			SpaceCoolingDemand			+= record.SpaceCoolingDemand;
		}
		public void Subtract(CoolingDemandRecord record)
		{
			InternalGains				-= record.InternalGains;
			SolarGainsTransmitted		-= record.SolarGainsTransmitted;
			SolarGainsNoTransmission	-= record.SolarGainsNoTransmission;
			SolarControl				-= record.SolarControl;
			TransmissionLosses			-= record.TransmissionLosses;
			VentilationLosses			-= record.VentilationLosses;
			VentilationNoCooling		-= record.VentilationNoCooling;
			RoomCoolCapacity			-= record.RoomCoolCapacity;
			TotalLosses					-= record.TotalLosses;
			GlazingRatio				-= record.GlazingRatio;
			AFactor						-= record.AFactor;
			CoolTimeConstant			-= record.CoolTimeConstant;
			CoolingReductionFactor		-= record.CoolingReductionFactor;
			SpaceCoolingDemand			-= record.SpaceCoolingDemand;
		}

		public void MultiplyBy(float factor)
		{
			InternalGains				*= factor;
			SolarGainsTransmitted		*= factor;
			SolarGainsNoTransmission	*= factor;
			SolarControl				*= factor;
			TransmissionLosses			*= factor;
			VentilationLosses			*= factor;
			VentilationNoCooling		*= factor;
			RoomCoolCapacity			*= factor;
			TotalLosses					*= factor;
			GlazingRatio				*= factor;
			AFactor						*= factor;
			CoolTimeConstant			*= factor;
			CoolingReductionFactor		*= factor;
			SpaceCoolingDemand			*= factor;
		}

		public CoolingDemandRecord Clone()
		{
			return new CoolingDemandRecord(
				Month, InternalGains, SolarGainsTransmitted, SolarGainsNoTransmission,
				SolarControl, TransmissionLosses, VentilationLosses, VentilationNoCooling,
				RoomCoolCapacity, TotalLosses, GlazingRatio,
				AFactor, CoolTimeConstant, CoolingReductionFactor, SpaceCoolingDemand
			);
		}
	}

}
