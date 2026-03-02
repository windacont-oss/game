#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "NebulaStrikeCharacter.generated.h"

class UHealthComponent;
class UWeaponComponent;
class UInputAction;
struct FInputActionValue;

UCLASS()
class NEBULASTRIKE_API ANebulaStrikeCharacter : public ACharacter
{
    GENERATED_BODY()

public:
    ANebulaStrikeCharacter();

protected:
    virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Components")
    UHealthComponent* HealthComponent;

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Components")
    UWeaponComponent* WeaponComponent;

    UPROPERTY(EditDefaultsOnly, Category = "Input")
    UInputAction* MoveAction;

    UPROPERTY(EditDefaultsOnly, Category = "Input")
    UInputAction* LookAction;

    UPROPERTY(EditDefaultsOnly, Category = "Input")
    UInputAction* FireAction;

private:
    void Move(const FInputActionValue& Value);
    void Look(const FInputActionValue& Value);
    void Fire();
};
