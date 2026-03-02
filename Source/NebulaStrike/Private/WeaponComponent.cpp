#include "WeaponComponent.h"
#include "DrawDebugHelpers.h"
#include "GameFramework/Actor.h"
#include "Kismet/GameplayStatics.h"

UWeaponComponent::UWeaponComponent()
{
    PrimaryComponentTick.bCanEverTick = false;
}

void UWeaponComponent::Fire()
{
    AActor* Owner = GetOwner();
    if (!Owner)
    {
        return;
    }

    FVector Start;
    FRotator Rotation;
    Owner->GetActorEyesViewPoint(Start, Rotation);
    const FVector End = Start + (Rotation.Vector() * Range);

    FHitResult Hit;
    FCollisionQueryParams QueryParams;
    QueryParams.AddIgnoredActor(Owner);

    if (GetWorld()->LineTraceSingleByChannel(Hit, Start, End, ECC_Visibility, QueryParams))
    {
        UGameplayStatics::ApplyPointDamage(
            Hit.GetActor(),
            Damage,
            Rotation.Vector(),
            Hit,
            Owner->GetInstigatorController(),
            Owner,
            DamageType
        );

        DrawDebugLine(GetWorld(), Start, Hit.Location, FColor::Red, false, 1.0f, 0, 1.0f);
    }
    else
    {
        DrawDebugLine(GetWorld(), Start, End, FColor::Blue, false, 1.0f, 0, 1.0f);
    }
}
