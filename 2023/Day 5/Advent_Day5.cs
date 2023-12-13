enum MapMode
{
    SeedToSoil,
    SoilToFertilizer,
    FertilizerToWater,
    WaterToLight,
    LightToTemperature,
    TemperatureToHumidity,
    HumidityToLocation,
    None
}

public class RangeTransform
{
    public long StartRange;
    public long EndRange;
    public long DestinationStart;
    public long DestinationEnd;

    public RangeTransform(long sourceStart, long destStart, long range)
    {
        StartRange = sourceStart;
        EndRange = sourceStart + range;
        DestinationStart = destStart;
        DestinationEnd = destStart + range;
    }

    public bool StartIsInRange(long val)
    {
        if (StartRange <= val && val <= EndRange)
        {
            return true;
        }
        return false;
    }

    public bool EndIsInRange(long val)
    {
        if (DestinationStart <= val && val <= DestinationEnd)
        {
            return true;
        }
        return false;
    }

    public long GetDestinationVal(long val)
    {
        long diff = val - StartRange;
        return DestinationStart + diff;
    }

    public long GetStartVal(long val)
    {
        long diff = val - DestinationStart;
        return StartRange + diff;
    }
}

private void AddValuesToDic(ref List<RangeTransform> dic, string[] nums)
{
    long destinationStart = 0, sourceStart = 0, range = 0;
    long.TryParse(nums[0], out destinationStart);
    long.TryParse(nums[1], out sourceStart);
    long.TryParse(nums[2], out range);
    dic.Add(new RangeTransform(sourceStart, destinationStart, range));
}

private void AdventDay5(object sender, MouseButtonEventArgs e)
{
    //Debug event to test various things; does nothing in release, but it's easier to leave it here for when it's needed later.
    var rawFile = File.ReadAllLines(@"AdventDay5Input.txt");
    List<string> input = new List<string>(rawFile);
    long finalVal = 0;
    List<string> seeds = new List<string>();
    List<RangeTransform> fullSeedRangeMap = new List<RangeTransform>();
    List<RangeTransform> seedToSoilMap = new List<RangeTransform>();
    List<RangeTransform> soilToFertilizerMap = new List<RangeTransform>();
    List<RangeTransform> fertilizerToWaterMap = new List<RangeTransform>();
    List<RangeTransform> waterToLightMap = new List<RangeTransform>();
    List<RangeTransform> lightToTemperatureMap = new List<RangeTransform>();
    List<RangeTransform> temperatureToHumidityMap = new List<RangeTransform>();
    List<RangeTransform> humidityToLocationMap = new List<RangeTransform>();
    MapMode mode = MapMode.None;
    foreach(string str in input)
    {
        string[] split = str.Split(':');
        switch(split[0])
        {
            case "seeds":
                string[] seedInput = split[1].Split(' ');
                foreach(string seed in seedInput)
                {
                    if (seed == "")
                    {
                        continue;
                    }
                    seeds.Add(seed);
                }
                break;
            case "seed-to-soil map":
                mode = MapMode.SeedToSoil;
                break;
            case "soil-to-fertilizer map":
                mode = MapMode.SoilToFertilizer;
                break;
            case "fertilizer-to-water map":
                mode = MapMode.FertilizerToWater;
                break;
            case "water-to-light map":
                mode = MapMode.WaterToLight;
                break;
            case "light-to-temperature map":
                mode = MapMode.LightToTemperature;
                break;
            case "temperature-to-humidity map":
                mode = MapMode.TemperatureToHumidity;
                break;
            case "humidity-to-location map":
                mode = MapMode.HumidityToLocation;
                break;
            case "":
                continue;
            default:
                {
                    string[] nums = split[0].Split(' ');
                    switch(mode)
                    {
                        case MapMode.SeedToSoil:
                            AddValuesToDic(ref seedToSoilMap, nums);
                            break;
                        case MapMode.SoilToFertilizer:
                            AddValuesToDic(ref soilToFertilizerMap, nums);
                            break;
                        case MapMode.FertilizerToWater:
                            AddValuesToDic(ref fertilizerToWaterMap, nums);
                            break;
                        case MapMode.WaterToLight:
                            AddValuesToDic(ref waterToLightMap, nums);
                            break;
                        case MapMode.LightToTemperature:
                            AddValuesToDic(ref lightToTemperatureMap, nums);
                            break;
                        case MapMode.TemperatureToHumidity:
                            AddValuesToDic(ref temperatureToHumidityMap, nums);
                            break;
                        case MapMode.HumidityToLocation:
                            AddValuesToDic(ref humidityToLocationMap, nums);
                            break;
                    }
                }
                break;
        }
    }
    //Translate seed values into their own range set
    for(int i = 0; i < seeds.Count; i+=2)
    {
        long rangeStart = 0, rangeLength = 0;
        long.TryParse(seeds[i], out rangeStart);
        long.TryParse(seeds[i + 1], out rangeLength);
        fullSeedRangeMap.Add(new RangeTransform(rangeStart, rangeStart, rangeLength));
    }
    //Everything is now translated
    //Process seeds to find lowest location
    // * Part 1 *
    /*finalVal = long.MaxValue;
    foreach (RangeTransform range in fullSeedRangeMap)
    {
        for (long seed = range.DestinationStart; seed < range.DestinationEnd; seed++)
        {
            long soil = 0, fert = 0, water = 0, light = 0, temp = 0, hum = 0, loc = 0;
            var index = seedToSoilMap.FindIndex(entry => entry.StartIsInRange(seed));
            if (index == -1)
            {
                soil = seed;
            }
            else
            {
                soil = seedToSoilMap[index].GetDestinationVal(seed);
            }
            index = soilToFertilizerMap.FindIndex(entry => entry.StartIsInRange(soil));
            if (index == -1)
            {
                fert = soil;
            }
            else
            {
                fert = soilToFertilizerMap[index].GetDestinationVal(soil);
            }
            index = fertilizerToWaterMap.FindIndex(entry => entry.StartIsInRange(fert));
            if (index == -1)
            {
                water = fert;
            }
            else
            {
                water = fertilizerToWaterMap[index].GetDestinationVal(fert);
            }
            index = waterToLightMap.FindIndex(entry => entry.StartIsInRange(water));
            if (index == -1)
            {
                light = water;
            }
            else
            {
                light = waterToLightMap[index].GetDestinationVal(water);
            }
            index = lightToTemperatureMap.FindIndex(entry => entry.StartIsInRange(light));
            if (index == -1)
            {
                temp = light;
            }
            else
            {
                temp = lightToTemperatureMap[index].GetDestinationVal(light);
            }
            index = temperatureToHumidityMap.FindIndex(entry => entry.StartIsInRange(temp));
            if (index == -1)
            {
                hum = temp;
            }
            else
            {
                hum = temperatureToHumidityMap[index].GetDestinationVal(temp);
            }
            index = humidityToLocationMap.FindIndex(entry => entry.StartIsInRange(hum));
            if (index == -1)
            {
                loc = hum;
            }
            else
            {
                loc = humidityToLocationMap[index].GetDestinationVal(hum);
            }
            if (finalVal > loc)
            {
                finalVal = loc;
            }
        }
    }*/
    // * Part 2 *
    //This chunks for about an eternity but does produce the correct result. Eventually.
    long highestDest = humidityToLocationMap.Max(entry => entry.DestinationEnd);
    for (long i = 0; i < highestDest; i++)
    {
        long seedlong = 0, soil = 0, fert = 0, water = 0, light = 0, temp = 0, hum = 0, loc = 0;
        loc = i;
        var index = humidityToLocationMap.FindIndex(entry => entry.EndIsInRange(loc));
        if (index == -1)
        {
            hum = loc;
        }
        else
        {
            hum = humidityToLocationMap[index].GetStartVal(loc);
        }
        index = temperatureToHumidityMap.FindIndex(entry => entry.EndIsInRange(hum));
        if (index == -1)
        {
            temp = hum;
        }
        else
        {
            temp = temperatureToHumidityMap[index].GetStartVal(hum);
        }
        index = lightToTemperatureMap.FindIndex(entry => entry.EndIsInRange(temp));
        if (index == -1)
        {
            light = temp;
        }
        else
        {
            light = lightToTemperatureMap[index].GetStartVal(temp);
        }
        index = waterToLightMap.FindIndex(entry => entry.EndIsInRange(light));
        if (index == -1)
        {
            water = light;
        }
        else
        {
            water = waterToLightMap[index].GetStartVal(light);
        }
        index = fertilizerToWaterMap.FindIndex(entry => entry.EndIsInRange(water));
        if (index == -1)
        {
            fert = water;
        }
        else
        {
            fert = fertilizerToWaterMap[index].GetStartVal(water);
        }
        index = soilToFertilizerMap.FindIndex(entry => entry.EndIsInRange(fert));
        if (index == -1)
        {
            soil = fert;
        }
        else
        {
            soil = soilToFertilizerMap[index].GetStartVal(fert);
        }
        index = seedToSoilMap.FindIndex(entry => entry.EndIsInRange(soil));
        if (index == -1)
        {
            seedlong = soil;
        }
        else
        {
            seedlong = seedToSoilMap[index].GetStartVal(soil);
        }
        index = fullSeedRangeMap.FindIndex(entry => entry.EndIsInRange(seedlong));
        if (index != -1)
        {
            finalVal = i;
            break;
        }
    }
    MessageBox.Show(finalVal.ToString());
}