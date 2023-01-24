using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ResourceTransformer : ResourceHolder
{
    [SerializeField] ResourceInstantiator _instantiator;
    [SerializeField] List<Recipe> _recipes;
    public Process process;

    private Dictionary<ResourceType, int> _storedResources = new Dictionary<ResourceType, int>();
    [SerializeField] int maxStoredResources = 10;
    [SerializeField] GameObject _resourcePrefab;

    private void Start()
    {
        InitStore();
    }

    public override bool IsAllowedToGive() => true;
    public override bool IsAllowedToReceive() => true;

    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder)
    {
        ResourceObject heldResource = resourceHolder.heldResource;
        if (heldResource == null)
            return true;

        ResourceType resourceType = heldResource.resourceType;
        return (
            resourceType != null
            && GetRecipe(resourceHolder.heldResource.resourceType) != null
            && !IsFull(resourceType)
        );
    }

    public override void OnTick()
    {
        if (heldResource != null)
        {
            ResourceType resourceType = heldResource.resourceType;
            if (GetRecipe(heldResource.resourceType) != null)
            {

                Store(resourceType);
                Destroy(heldResource.gameObject);
                heldResource = null;
            }
        }
        foreach (Recipe recipe in _recipes)
        {
            if (CanProcess(recipe))
                Process(recipe);
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

    private bool IsFull(ResourceType resourceType)
    {
        return (
            _storedResources.ContainsKey(resourceType)
            && _storedResources[resourceType] == maxStoredResources
        );
    }

    private void Store(ResourceType resourceType, int count = 1) => _storedResources[resourceType] += count;

    private bool CanProcess(Recipe recipe)
    {
        bool hasIngredients = recipe.ingredients.All(
            ingredient => ingredient.count <= _storedResources[ingredient.type]
        );
        return hasIngredients && !IsFull(recipe.product.type);
    }

    private void Process(Recipe recipe)
    {
        foreach (ResourceCount ingredient in recipe.ingredients)
            _storedResources[ingredient.type] -= ingredient.count;
        Store(recipe.product.type, recipe.product.count);
    }


    private void InstantiateOutputResource()
    {
        if (heldResource == null)
        {
            foreach (Recipe recipe in _recipes)
            {
                ResourceType resourceType = recipe.product.type;
                if (_storedResources[resourceType] > 0)
                {
                    _storedResources[resourceType]--;
                    _instantiator.InstantiateResource(resourceType);
                    break;
                }
            }
        }
    }

    private void InitStore()
    {
        foreach (Recipe recipe in _recipes)
        {
            _storedResources[recipe.product.type] = 0;
            foreach (ResourceCount resourceCount in recipe.ingredients)
                _storedResources[resourceCount.type] = 0;
        }
    }
}
