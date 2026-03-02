#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "HealthComponent.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnDeathSignature, AActor*, Victim);

UCLASS(ClassGroup=(Gameplay), meta=(BlueprintSpawnableComponent))
class NEBULASTRIKE_API UHealthComponent : public UActorComponent
{
    GENERATED_BODY()

public:
    UHealthComponent();

protected:
    virtual void BeginPlay() override;

    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Health")
    float MaxHealth = 100.0f;

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Health")
    float CurrentHealth = 100.0f;

public:
    UPROPERTY(BlueprintAssignable, Category = "Health")
    FOnDeathSignature OnDeath;

    UFUNCTION(BlueprintCallable, Category = "Health")
    void ApplyDamage(float DamageAmount);

    UFUNCTION(BlueprintPure, Category = "Health")
    float GetHealthPercent() const;
};
