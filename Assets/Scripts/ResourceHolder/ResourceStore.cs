using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceStore : MonoBehaviour
{
    [SerializeField] int _maxStoredResources = 10;
    private Dictionary<ResourceType, int> _storedResources = new Dictionary<ResourceType, int>();

    public void Initialize(List<Recipe> recipes)
    {
        foreach (Recipe recipe in recipes)
        {
            _storedResources[recipe.product.type] = 0;
            foreach (ResourceCount resourceCount in recipe.ingredients)
                _storedResources[resourceCount.type] = 0;
        }
    }

    public bool CanStore(ResourceType type, int amount = 1)
    {
        return _storedResources[type] + amount <= _maxStoredResources;
    }

    public void Store(ResourceType type, int amount = 1)
    {
        _storedResources[type] += amount;
    }

    public bool CanProcess(Recipe recipe)
    {
        bool hasIngredients = recipe.ingredients.All(
            ingredient => ingredient.count <= _storedResources[ingredient.type]
        );
        return hasIngredients && CanStore(recipe.product.type, recipe.product.count);
    }

    public void Process(Recipe recipe)
    {
        foreach (ResourceCount ingredient in recipe.ingredients)
            _storedResources[ingredient.type] -= ingredient.count;
        Store(recipe.product.type, recipe.product.count);
    }

    public bool CanWithdraw(ResourceType type, int amount = 1)
    {
        return _storedResources[type] >= amount;
    }

    public void Withdraw(ResourceType type, int amount = 1)
    {
        _storedResources[type] -= amount;
    }


}
