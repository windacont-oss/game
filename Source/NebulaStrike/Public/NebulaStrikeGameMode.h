#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "NebulaStrikeGameMode.generated.h"

UCLASS()
class NEBULASTRIKE_API ANebulaStrikeGameMode : public AGameModeBase
{
    GENERATED_BODY()

public:
    ANebulaStrikeGameMode();

protected:
    virtual void BeginPlay() override;

    UPROPERTY(EditDefaultsOnly, Category = "Flow")
    int32 TargetKillsToWin = 20;

private:
    int32 CurrentKills = 0;

public:
    void RegisterKill();
};
