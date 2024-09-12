namespace DictionaryOfLists.Tests;

public class GetValueOrAddTest
{
    [Fact]
    public void GetValueOrAdd_Gets()
    {
        // Arrange
        var dict = new Dictionary<int, int>
                   {
                       [0] = 0,
                       [1] = 1,
                       [2] = 2
                   };

        // Act
        var val = dict.GetValueOrAdd(2);

        // Assert
        Assert.Equal(2, val);
    }

    [Fact]
    public void GetValueOrAdd_Adds()
    {
        // Arrange
        var dict = new Dictionary<int, int>
                   {
                       [0] = 0,
                       [1] = 1,
                       [2] = 2
                   };

        // Act
        var val = dict.GetValueOrAdd(3);

        // Assert
        Assert.Contains(3, dict.Keys);
        Assert.Equal(0, val);
    }

    [Fact]
    public void GetValueOrAdd_ClassType_Adds()
    {
        // Arrange
        var dict = new Dictionary<int, List<int>>
                   {
                       [0] = [0,100],
                       [1] = [1,101],
                       [2] = [2,102]
                   };

        // Act
        var val = dict.GetValueOrAdd(3);

        // Assert
        Assert.Contains(3, dict.Keys);
        Assert.Empty(val);
    }
}