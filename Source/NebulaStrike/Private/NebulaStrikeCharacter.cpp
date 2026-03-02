#include "NebulaStrikeCharacter.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "GameFramework/PlayerController.h"
#include "HealthComponent.h"
#include "InputActionValue.h"
#include "WeaponComponent.h"

ANebulaStrikeCharacter::ANebulaStrikeCharacter()
{
    PrimaryActorTick.bCanEverTick = false;

    HealthComponent = CreateDefaultSubobject<UHealthComponent>(TEXT("HealthComponent"));
    WeaponComponent = CreateDefaultSubobject<UWeaponComponent>(TEXT("WeaponComponent"));
}

void ANebulaStrikeCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
    Super::SetupPlayerInputComponent(PlayerInputComponent);

    if (UEnhancedInputComponent* EnhancedInput = Cast<UEnhancedInputComponent>(PlayerInputComponent))
    {
        if (MoveAction) EnhancedInput->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ANebulaStrikeCharacter::Move);
        if (LookAction) EnhancedInput->BindAction(LookAction, ETriggerEvent::Triggered, this, &ANebulaStrikeCharacter::Look);
        if (FireAction) EnhancedInput->BindAction(FireAction, ETriggerEvent::Started, this, &ANebulaStrikeCharacter::Fire);
    }
}

void ANebulaStrikeCharacter::Move(const FInputActionValue& Value)
{
    const FVector2D Input = Value.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), Input.Y);
    AddMovementInput(GetActorRightVector(), Input.X);
}

void ANebulaStrikeCharacter::Look(const FInputActionValue& Value)
{
    const FVector2D Input = Value.Get<FVector2D>();
    AddControllerYawInput(Input.X);
    AddControllerPitchInput(Input.Y);
}

void ANebulaStrikeCharacter::Fire()
{
    if (WeaponComponent)
    {
        WeaponComponent->Fire();
    }
}
