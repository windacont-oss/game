#include "NebulaStrikeGameMode.h"
#include "Engine/Engine.h"

ANebulaStrikeGameMode::ANebulaStrikeGameMode()
{
    DefaultPawnClass = nullptr;
}

void ANebulaStrikeGameMode::BeginPlay()
{
    Super::BeginPlay();

    if (GEngine)
    {
        GEngine->AddOnScreenDebugMessage(-1, 5.0f, FColor::Cyan, TEXT("Nebula Strike initialized. Objective: eliminate hostiles."));
    }
}

void ANebulaStrikeGameMode::RegisterKill()
{
    ++CurrentKills;

    if (CurrentKills >= TargetKillsToWin && GEngine)
    {
        GEngine->AddOnScreenDebugMessage(-1, 8.0f, FColor::Green, TEXT("Mission complete: sector secure."));
    }
}
