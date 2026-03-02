#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "WeaponComponent.generated.h"

class UNiagaraSystem;

UCLASS(ClassGroup=(Gameplay), meta=(BlueprintSpawnableComponent))
class NEBULASTRIKE_API UWeaponComponent : public UActorComponent
{
    GENERATED_BODY()

public:
    UWeaponComponent();

    UFUNCTION(BlueprintCallable, Category = "Weapon")
    void Fire();

protected:
    UPROPERTY(EditAnywhere, Category = "Weapon")
    float Damage = 25.0f;

    UPROPERTY(EditAnywhere, Category = "Weapon")
    float Range = 10000.0f;

    UPROPERTY(EditAnywhere, Category = "Weapon")
    TSubclassOf<UDamageType> DamageType;

    UPROPERTY(EditAnywhere, Category = "VFX")
    UNiagaraSystem* MuzzleFlash = nullptr;
};
