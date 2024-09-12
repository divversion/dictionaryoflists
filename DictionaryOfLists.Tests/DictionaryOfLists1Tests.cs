namespace DictionaryOfLists.Tests;

public class DictionaryOfLists1Tests
{
    [Fact]
    private void CopyConstructor_Copies()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var listDict2 = new DictionaryOfLists1<int, string>(listDict);

        // Assert
        Assert.True(listDict2.ContainsKey(1));
        Assert.Equal(listPre, listDict2[1]);
    }
    
    [Fact]
    private void GetEnumerator_Enumerates()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var keysEnumerated = new List<int>();
        foreach (var kv in listDict)
            keysEnumerated.Add(kv.Key);

        // Assert
        Assert.Equal(new List<int> { 1, 2 }, keysEnumerated);
    }

    [Fact]
    private void Add_AddsKey()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();

        // Act
        listDict.Add(1, listPre);

        // Assert
        Assert.True(listDict.TryGetValue(1, out _));
    }

    [Fact]
    private void Add_StoresList()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();

        // Act
        listDict.Add(1, listPre);
        listDict.TryGetValue(1, out var storedList);

        // Assert
        Assert.Equal(listPre, storedList);
    }

    [Fact]
    private void Add_ExistingKey_Throws()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var exception = Record.Exception(() => listDict.Add(1, listPre));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    private void Add_ExistingKey_NewListEmpty_Throws()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var exception = Record.Exception(() => listDict.Add(1, []));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    private void Add_ExistingKeyButEmpty_StoresList()
    {
        // Arrange
        var listPre = new List<string>();
        var listDict = new DictionaryOfLists1<int, string>();

        // Act
        listDict.Add(1, listPre);
        listDict.TryGetValue(1, out var storedList);

        // Assert
        Assert.Equal(listPre, storedList);
    }

    [Fact]
    private void Add_EmptyList_DoesNotThrow()
    {
        // Arrange
        var listPre = new List<string>();
        var listDict = new DictionaryOfLists1<int, string>();

        // Act
        var exception = Record.Exception(() => listDict.Add(1, listPre));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    private void ContainsKey_ReportsTrueForContained()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var isContained = listDict.ContainsKey(1);

        // Assert
        Assert.True(isContained);
    }

    [Fact]
    private void ContainsKey_ReportsFalseForNotContained()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var isContained = listDict.ContainsKey(2);

        // Assert
        Assert.False(isContained);
    }

    [Fact]
    private void ContainsKey_ReportsFalseForEmptyList()
    {
        // Arrange
        var listPre = new List<string>();
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var contains = listDict.ContainsKey(1);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    private void CopyTo_Copies()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var array = new KeyValuePair<int, IList<string>>[2];
        ((ICollection<KeyValuePair<int, IList<string>>>)listDict).CopyTo(array, 0);

        // Assert
        Assert.Equal(1, array[0].Key);
        Assert.Equal(listPre, array[0].Value);
        Assert.Equal(2, array[1].Key);
        Assert.Equal(list2, array[1].Value);
    }

    [Fact]
    private void Remove_Removes()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        listDict.Remove(1);

        // Assert
        Assert.False(listDict.ContainsKey(1));
    }

    [Fact]
    private void Remove_IsEmptyListAfterwards()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        listDict.Remove(1);

        // Assert
        Assert.Empty(listDict[1]);
    }

    [Fact]
    private void Remove_ReducesTotalCapacity()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var capacityBefore = listDict.TotalCapacity;
        listDict.Remove(1);

        // Assert
        Assert.True(listDict.TotalCapacity < capacityBefore);
    }

    [Fact]
    private void Clear_Clears()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        listDict.Clear();

        // Assert
        Assert.False(listDict.ContainsKey(1));
    }

    [Fact]
    private void Count_Correct()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var count = listDict.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    private void TryGetValue_ExistingKey_ReportsTrue()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var result = listDict.TryGetValue(1, out _);

        // Assert
        Assert.True(result);
    }

    [Fact]
    private void TryGetValue_NonExistingKey_ReportsFalse()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var result = listDict.TryGetValue(42, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    private void TryGetValue_ExistingKey_OutputsStoredList()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        listDict.TryGetValue(1, out var storedList);

        // Assert
        Assert.Equal(listPre, storedList);
    }

    [Fact]
    private void TryGetValue_ExistingEmptyList_ReportsFalse()
    {
        // Arrange
        var listPre = new List<string>();
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var result = listDict.TryGetValue(1, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    private void Indexer_ExistingKey_OutputsStoredList()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var storedList = listDict[1];

        // Assert
        Assert.Equal(listPre, storedList);
    }

    [Fact]
    private void Indexer_NonExistingKey_DoesNotThrow()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var exception = Record.Exception(() => listDict[42]);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    private void Indexer_NonExistingKey_OutputsEmptyList()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var storedList = listDict[42];

        // Assert
        Assert.Empty(storedList);
    }

    [Fact]
    private void Indexer_NonExistingKey_ThenModified_Stores()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var storedList = listDict[42];
        storedList.Add("X");
        var storedList2 = listDict[42];

        // Assert
        Assert.Equal(storedList, storedList2);
    }

    [Fact]
    private void Keys_ContainsAllKeys()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var storedKeys = listDict.Keys;

        // Assert
        Assert.Equal(new List<int> { 1, 2 }, storedKeys);
    }

    [Fact]
    private void Values_ContainsAllValues()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var list2 = new List<string> { "D", "E", "F" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        listDict.Add(2, list2);

        // Act
        var storedValues = listDict.Values;

        // Assert
        Assert.Equal(new List<List<string>> { listPre, list2 }, storedValues);
    }

    [Fact]
    private void ListListProxy_Add_Adds()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Add("D");

        // Assert
        var list2 = listDict[1];
        Assert.Equal(new List<string> { "A", "B", "C", "D" }, list2);
    }

    [Fact]
    private void ListListProxy_AddToEmpty_AddsToDict()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);

        // Act
        var list = listDict[42];
        list.Add("X");

        // Assert
        Assert.True(listDict.TryGetValue(42, out _));
    }

    [Fact]
    private void ListListProxy_Insert_Inserts()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Insert(1, "X");

        // Assert
        var list2 = listDict[1];
        Assert.Equal(new List<string> { "A", "X", "B", "C" }, list2);
    }

    [Fact]
    private void ListListProxy_Remove_Removes()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Remove("A");

        // Assert
        var list2 = listDict[1];
        Assert.Equal(new List<string> { "B", "C" }, list2);
    }

    [Fact]
    private void ListListProxy_Remove_ThenEmpty_RemovesFromDict()
    {
        // Arrange
        var listPre = new List<string> { "A" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Remove("A");

        // Assert
        Assert.False(listDict.TryGetValue(1, out _));
    }

    [Fact]
    private void ListListProxy_RemoveAt_Removes()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.RemoveAt(0);

        // Assert
        var list2 = listDict[1];
        Assert.Equal(new List<string> { "B", "C" }, list2);
    }

    [Fact]
    private void ListListProxy_RemoveAt_ThenEmpty_RemovesFromDict()
    {
        // Arrange
        var listPre = new List<string> { "A" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.RemoveAt(0);

        // Assert
        Assert.False(listDict.TryGetValue(1, out _));
    }

    [Fact]
    private void ListListProxy_Clear_Clears()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Clear();

        // Assert
        Assert.False(listDict.TryGetValue(1, out _));
    }

    [Fact]
    private void ListListProxy_Indexer_Stores()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list[0] = "X";

        // Assert
        var list2 = listDict[1];
        Assert.Equal(new List<string> { "X", "B", "C" }, list2);
    }

    [Fact]
    private void ListListProxy_ClearAndAdd_HasValue()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Clear();
        list.Add("A");

        // Assert
        Assert.True(listDict.TryGetValue(1, out _));
    }
    
    [Fact]
    private void ListListProxy_ClearAndAdd_ListCorrect()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists1<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Clear();
        list.Add("A");

        // Assert
        Assert.Equal(["A"], listDict[1]);
    }

    [Fact]
    private void ListListProxy_ClearAndAddBothInDictAndListRef_ListCorrect()
    {
        // Arrange
        var listPre = new List<string> { "A", "B", "C" };
        var listDict = new DictionaryOfLists3<int, string>();
        listDict.Add(1, listPre);
        var list = listDict[1];

        // Act
        list.Clear();
        listDict[1].Add("A");
        list.Add("B");

        // Assert
        Assert.Equal(["A", "B"], listDict[1]);
    }

    // Listlist.Contains(), .IndexOf(), .CopyTo(), .Count(), .IsReadOnly() are pure read-only forwards, so not tested
}