const summer = require('./sum');

test('Test sum function:', () => {
  expect(summer(1, 2)).toBe(3);
  expect(summer(15, 50)).toBe(65);
});
