# qs

A querystring parsing and stringifying library with some added security.

[![Build Status](https://api.travis-ci.org/ljharb/qs.svg)](http://travis-ci.org/ljharb/qs)

Lead Maintainer: [Jordan Harband](https://github.com/ljharb)

The **qs** module was originally created and maintained by [TJ Holowaychuk](https://github.com/visionmedia/node-querystring).

## Usage

```javascript
var qs = require('qs');
var assert = require('assert');

var obj = qs.parse('a=c');
assert.deepEqual(obj, { a: 'c' });

var str = qs.stringify(obj);
assert.equal(str, 'a=c');
```

### Parsing Objects

[](#preventEval)
```javascript
qs.parse(string, [options]);
```

**qs** allows you to create nested objects within your query strings, by surrounding the name of sub-keys with square brackets `[]`.
For example, the string `'foo[bar]=baz'` converts to:

```javascript
assert.deepEqual(qs.parse('foo[bar]=baz'), {
  foo: {
    bar: 'baz'
  }
});
```

When using the `plainObjects` option the parsed value is returned as a plain object, created via `Object.create(null)` and as such you should be aware that prototype methods will not exist on it and a user may set those names to whatever value they like:

```javascript
var plainObject = qs.parse('a[hasOwnProperty]=b', { plainObjects: true });
assert.deepEqual(plainObject, { a: { hasOwnProperty: 'b' } });
```

By default parameters that would overwrite properties on the object prototype are ignored, if you wish to keep the data from those fields either use `plainObjects` as mentioned above, or set `allowPrototypes` to `true` which will allow user input to overwrite those properties. *WARNING* It is generally a bad idea to enable this option as it can cause problems when attempting to use the properties that have been overwritten. Always be careful with this option.

```javascript
var protoObject = qs.parse('a[hasOwnProperty]=b', { allowPrototypes: true });
assert.deepEqual(protoObject, { a: { hasOwnProperty: 'b' } });
```

URI encoded strings work too:

```javascript
assert.deepEqual(qs.parse('a%5Bb%5D=c'), {
  a: { b: 'c' }
});
```

You can also nest your objects, like `'foo[bar][baz]=foobarbaz'`:

```javascript
assert.deepEqual(qs.parse('foo[bar][baz]=foobarbaz'), {
  foo: {
    bar: {
      baz: 'foobarbaz'
    }
  }
});
```

By default, when nesting objects **qs** will only parse up to 5 children deep. This means if you attempt to parse a string like
`'a[b][c][d][e][f][g][h][i]=j'` your resulting object will be:

```javascript
var expected = {
  a: {
    b: {
      c: {
        d: {
          e: {
            f: {
              '[g][h][i]': 'j'
            }
          }
        }
      }
    }
  }
};
var string = 'a[b][c][d][e][f][g][h][i]=j';
assert.deepEqual(qs.parse(string), expected);
```

This depth can be overridden by passing a `depth` option to `qs.parse(string, [options])`:

```javascript
var deep = qs.parse('a[b][c][d][e][f][g][h][i]=j', { depth: 1 });
assert.deepEqual(deep, { a: { b: { '[c][d][e][f][g][h][i]': 'j' } } });
```

The depth limit helps mitigate abuse when **qs** is used to parse user input, and it is recommended to keep it a reasonably small number.

For similar reasons, by default **qs** will only parse up to 1000 parameters. This can be overridden by passing a `parameterLimit` option:

```javascript
var limited = qs.parse('a=b&c=d', { parameterLimit: 1 });
assert.deepEqual(limited, { a: 'b' });
```

An optional delimiter can also be passed:

```javascript
var delimited = qs.parse('a=b;c=d', { delimiter: ';' });
assert.deepEqual(delimited, { a: 'b', c: 'd' });
```

Delimiters can be a regular expression too:

```javascript
var regexed = qs.parse('a=b;c=d,e=f', { delimiter: /[;,]/ });
assert.deepEqual(regexed, { a: 'b', c: 'd', e: 'f' });
```

Option `allowDots` can be used to enable dot notation:

```javascript
var withDots = qs.parse('a.b=c', { allowDots: true });
assert.deepEqual(withDots, { a: { b: 'c' } });
```

### Parsing Arrays

**qs** can also parse arrays using a similar `[]` notation:

```javascript
var withArray = qs.parse('a[]=b&a[]=c');
assert.deepEqual(withArray, { a: ['b', 'c'] });
```

You may specify an index as well:

```javascript
var withIndexes = qs.parse('a[1]=c&a[0]=b');
assert.deepEqual(withIndexes, { a: ['b', 'c'] });
```

Note that the only difference between an index in an array and a key in an object is that the value between the brackets must be a number
to create an array. When creating arrays with specific indices, **qs** will compact a sparse array to only the existing values preserving
their order:

```javascript
var noSparse = qs.parse('a[1]=b&a[15]=c');
assert.deepEqual(noSparse, { a: ['b', 'c'] });
```

Note that an empty string is also a value, and will be preserved:

```javascript
var withEmptyString = qs.parse('a[]=&a[]=b');
assert.deepEqual(withEmptyString, { a: ['', 'b'] });

var withIndexedEmptyString = qs.parse('a[0]=b&a[1]=&a[2]=c');
assert.deepEqual(withIndexedEmptyString, { a: ['b', '', 'c'] });
```

**qs** will also limit specifying indices in an array to a maximum index of `20`. Any array members with an index of greater than `20` will
instead be converted to an object with the index as the key:

```javascript
var withMaxIndex = qs.parse('a[100]=b');
assert.deepEqual(withMaxIndex, { a: { '100': 'b' } });
```

This limit can be overridden by passing an `arrayLimit` option:

```javascript
var withArrayLimit = qs.parse('a[1]=b', { arrayLimit: 0 });
assert.deepEqual(withArrayLimit, { a: { '1': 'b' } });
```

To disable array parsing entirely, set `parseArrays` to `false`.

```javascript
var noParsingArrays = qs.parse('a[]=b', { parseArrays: false });
assert.deepEqual(noParsingArrays, { a: { '0': 'b' } });
```

If you mix notations, **qs** will merge the two items into an object:

```javascript
var mixedNotation = qs.parse('a[0]=b&a[b]=c');
assert.deepEqual(mixedNotation, { a: { '0': 'b', b: 'c' } });
```

You can also create arrays of objects:

```javascript
var arraysOfObjects = qs.parse('a[][b]=c');
assert.deepEqual(arraysOfObjects, { a: [{ b: 'c' }] });
```

### Stringifying

[](#preventEval)
```javascript
qs.stringify(object, [options]);
```

When stringifying, **qs** by default URI encodes output. Objects are stringified as you would expect:

```javascript
assert.equal(qs.stringify({ a: 'b' }), 'a=b');
assert.equal(qs.stringify({ a: { b: 'c' } }), 'a%5Bb%5D=c');
```

This encoding can be disabled by setting the `encode` option to `false`:

```javascript
var unencoded = qs.stringify({ a: { b: 'c' } }, { encode: false });
assert.equal(unencoded, 'a[b]=c');
```

This encoding can also be replaced by a custom encoding method set as `encoder` option:

```javascript
var encoded = qs.stringify({ a: { b: 'c' } }, { encoder: function (str) {
  // Passed in values `a`, `b`, `c`
  return // Return encoded string
}})
```

_(Note: the `encoder` option does not apply if `encode` is `false`)_

Analogue to the `encoder` there is a `decoder` option for `parse` to override decoding of properties and values:

```javascript
var decoded = qs.parse('x=z', { decoder: function (str) {
  // Passed in values `x`, `z`
  return // Return decoded string
}})
```

Examples beyond this point will be shown as though the output is not URI encoded for clarity. Please note that the return values in these cases *will* be URI encoded during real usage.

When arrays are stringified, by default they are given explicit indices:

```javascript
qs.stringify({ a: ['b', 'c', 'd'] });
// 'a[0]=b&a[1]=c&a[2]=d'
```

You may override this by setting the `indices` option to `false`:

```javascript
qs.stringify({ a: ['b', 'c', 'd'] }, { indices: false });
// 'a=b&a=c&a=d'
```

You may use the `arrayFormat` option to specify the format of the output array

```javascript
qs.stringify({ a: ['b', 'c'] }, { arrayFormat: 'indices' })
// 'a[0]=b&a[1]=c'
qs.stringify({ a: ['b', 'c'] }, { arrayFormat: 'brackets' })
// 'a[]=b&a[]=c'
qs.stringify({ a: ['b', 'c'] }, { arrayFormat: 'repeat' })
// 'a=b&a=c'
```

Empty strings and null values will omit the value, but the equals sign (=) remains in place:

```javascript
assert.equal(qs.stringify({ a: '' }), 'a=');
```

Properties that are set to `undefined` will be omitted entirely:

```javascript
assert.equal(qs.stringify({ a: null, b: undefined }), 'a=');
```

The delimiter may be overridden with stringify as well:

```javascript
assert.equal(qs.stringify({ a: 'b', c: 'd' }, { delimiter: ';' }), 'a=b;c=d');
```

Finally, you can use the `filter` option to restrict which keys will be included in the stringified output.
If you pass a function, it will be called for each key to obtain the replacement value. Otherwise, if you
pass an array, it will be used to select properties and array indices for stringification:

```javascript
function filterFunc(prefix, value) {
  if (prefix == 'b') {
    // Return an `undefined` value to omit a property.
    return;
  }
  if (prefix == 'e[f]') {
    return value.getTime();
  }
  if (prefix == 'e[g][0]') {
    return value * 2;
  }
  return value;
}
qs.stringify({ a: 'b', c: 'd', e: { f: new Date(123), g: [2] } }, { filter: filterFunc });
// 'a=b&c=d&e[f]=123&e[g][0]=4'
qs.stringify({ a: 'b', c: 'd', e: 'f' }, { filter: ['a', 'e'] });
// 'a=b&e=f'
qs.stringify({ a: ['b', 'c', 'd'], e: 'f' }, { filter: ['a', 0, 2] });
// 'a[0]=b&a[2]=d'
```

### Handling of `null` values

By default, `null` values are treated like empty strings:

```javascript
var withNull = qs.stringify({ a: null, b: '' });
assert.equal(withNull, 'a=&b=');
```

Parsing does not distinguish between parameters with and without equal signs. Both are converted to empty strings.

```javascript
var equalsInsensitive = qs.parse('a&b=');
assert.deepEqual(equalsInsensitive, { a: '', b: '' });
```

To distinguish between `null` values and empty strings use the `strictNullHandling` flag. In the result string the `null`
values have no `=` sign:

```javascript
var strictNull = qs.stringify({ a: null, b: '' }, { strictNullHandling: true });
assert.equal(strictNull, 'a&b=');
```

To parse values without `=` back to `null` use the `strictNullHandling` flag:

```javascript
var parsedStrictNull = qs.parse('a&b=', { strictNullHandling: true });
assert.deepEqual(parsedStrictNull, { a: null, b: '' });
```

To completely skip rendering keys with `null` values, use the `skipNulls` flag:

```javascript
var nullsSkipped = qs.stringify({ a: 'b', c: null}, { skipNulls: true });
assert.equal(nullsSkipped, 'a=b');
```

### Dealing with special character sets

By default the encoding and decoding of characters is done in `utf-8`. If you 
wish to encode querystrings to a different character set (i.e.
[Shift JIS](https://en.wikipedia.org/wiki/Shift_JIS)) you can use the
[`qs-iconv`](https://github.com/martinheidegger/qs-iconv) library:

```javascript
var encoder = require('qs-iconv/encoder')('shift_jis');
var shiftJISEncoded = qs.stringify({ a: '??????????????????' }, { encoder: encoder });
assert.equal(shiftJISEncoded, 'a=%82%B1%82%F1%82%C9%82%BF%82%CD%81I');
```

This also works for decoding of query strings:

```javascript
var decoder = require('qs-iconv/decoder')('shift_jis');
var obj = qs.parse('a=%82%B1%82%F1%82%C9%82%BF%82%CD%81I', { decoder: decoder });
assert.deepEqual(obj, { a: '??????????????????' });
```
