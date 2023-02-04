using System.Collections.Generic;
using UnityEngine;
public class ResourceTransformer : ResourceHolder
{
    [SerializeField] ResourceInstantiator _instantiator;
    [SerializeField] ResourceStore _store;
    [SerializeField] List<Recipe> _recipes;
    public Process process;

    public override bool IsAllowedToGive() => true;

    private void Start()
    {
        _store.Initialize(_recipes);
    }

    public override bool IsAllowedToReceiveFromDynamic(ResourceHolder resourceHolder)
    {
        ResourceObject resource = resourceHolder.heldResource;
        return resource == null || (
            resource.resourceType != null
            && GetRecipe(resource.resourceType) != null
            && _store.CanStore(resource.resourceType, 1)
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
                SetHeldResource(null);
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
