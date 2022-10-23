# Hero

## Extra Credit

Key to enable hero destruction if the hero hits 5 enemies. HeroBehavior.cs has the method HasLives, which returns true if hero destruction is disabled or if the number of lives is greater than 0. If there are no lives, the hero is destroyed. This is done in the Update function of HeroBehavior.cs.


### HeroBehavior.cs Update Function

``` csharp
void Update()
{
    ...
    
    if (!GameManager.Instance.HasLives())
    {
        Destroy(gameObject);
    }

    ...
}
```

### GameManager.cs
``` csharp
public bool HasLives()
{
    return !survivalMode || numberOfLives > 0;
}
```