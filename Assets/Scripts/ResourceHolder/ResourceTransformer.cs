using System.Collections.Generic;
using UnityEngine;
public class ResourceTransformer : ResourceHolder
{
    [SerializeField] ResourceInstantiator _instantiator;
    [SerializeField] ResourceStore _store;
    [SerializeField] List<Recipe> _recipes;
    public Process process;

    public override bool IsAllowedToGive() => true;
    public override bool IsAllowedToReceive() => true;

    private void Start()
    {
        _store.Initialize(_recipes);
    }

    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder)
    {
        ResourceObject heldResource = resourceHolder.heldResource;
        if (heldResource == null)
            return true;

        ResourceType resourceType = heldResource.resourceType;
        return (
            resourceType != null
            && GetRecipe(resourceHolder.heldResource.resourceType) != null
            && _store.CanStore(resourceType, 1)
        );
    }

    public override void OnTick()
    {
        if (heldResource != null)
        {
            ResourceType resourceType = heldResource.resourceType;
            if (GetRecipe(heldResource.resourceType) != null)
            {

                _store.Store(resourceType, 1);
                Destroy(heldResource.gameObject);
                heldResource = null;
            }
        }
        foreach (Recipe recipe in _recipes)
        {
            if (_store.CanProcess(recipe))
                _store.Process(recipe);
        }

        InstantiateOutputResource();
    }


    public Recipe GetRecipe(ResourceType resourceType)
    {
        if (resourceType != null)
            foreach (Recipe recipe in _recipes)
                foreach (ResourceCount resourceCount in recipe.ingredients)
                    if (resourceCount.type == resourceType)
                        return recipe;
        return null;
    }
    private void InstantiateOutputResource()
    {
        if (heldResource == null)
        {
            foreach (Recipe recipe in _recipes)
            {
                ResourceType resourceType = recipe.product.type;
                if (_store.CanWithdraw(resourceType, 1))
                {
                    _store.Withdraw(resourceType, 1);
                    _instantiator.InstantiateResource(resourceType);
                    break;
                }
            }
        }
    }
}
