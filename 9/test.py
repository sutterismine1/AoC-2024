# Example usage
str1 = open("right.txt").read()
str2 = open("wrong.txt").read()

def compare_strings(str1, str2):
    for i, (c1, c2) in enumerate(zip(str1, str2)):
        if c1 != c2:
            yield i, c1, c2

for idx, c1, c2 in compare_strings(str1, str2):
    print(f"Difference at index {idx}: '{c1}' vs '{c2}'")

# Check if one string is longer
if len(str1) != len(str2):
    print(f"Strings have different lengths: {len(str1)} vs {len(str2)}")
