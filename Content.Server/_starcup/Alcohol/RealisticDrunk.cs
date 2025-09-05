using System.Linq;
using Content.Server.Body.Components;
using Content.Shared.Drunk;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server._starcup.Alcohol;

public sealed partial class RealisticDrunk : EntityEffect
{
    /// <summary>
    /// Base number of seconds to add to Drunk status effect upon each metabolic action
    /// </summary>
    [DataField]
    public float BoozePower = 3f;

    /// <summary>
    /// Should speech be slurred from this effect? Yea, duh.
    /// </summary>
    [DataField]
    public bool SlurSpeech = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("reagent-effect-guidebook-drunk", ("chance", Probability));
    }

    public override void Effect(EntityEffectBaseArgs args)
    {
        var boozePower = BoozePower;

        if (args is EntityEffectReagentArgs reagentArgs)
        {
            var totalEthanolQuantity = reagentArgs.Source!.Contents.First(x => x.Reagent.Prototype == "Ethanol").Quantity;
            if (args.EntityManager.TryGetComponent<MetabolizerComponent>(reagentArgs.OrganEntity, out var metabolizer))
            {
                var group = metabolizer.MetabolismGroups!.First(x => x.Id == "Alcohol");
                var realRate = 0.5f * group.MetabolismRateModifier;  // Ethanol metabolism rate * organ metabolism rate

                if (totalEthanolQuantity >= realRate)
                {
                    boozePower *= totalEthanolQuantity.Float();
                }
                else
                {
                    boozePower *= reagentArgs.Scale.Float();
                }
            }
            else
            {
                boozePower *= reagentArgs.Scale.Float();
            }
        }

        var drunkSys = args.EntityManager.EntitySysManager.GetEntitySystem<SharedDrunkSystem>();
        drunkSys.TryApplyDrunkenness(args.TargetEntity, boozePower);
    }
}
