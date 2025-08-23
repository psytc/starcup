using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Client._starcup.Alcohol;

public sealed partial class RealisticDrunk : EntityEffect
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-drunk", ("chance", Probability));

    public override void Effect(EntityEffectBaseArgs args)
    {
    }
}
