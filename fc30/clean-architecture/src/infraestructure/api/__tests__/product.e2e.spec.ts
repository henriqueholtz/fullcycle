import { app, sequelize } from '../express';
import request from 'supertest';

describe('E2E test for products', () => {
  beforeEach(async () => {
    await sequelize.sync({ force: true });
  });

  afterAll(async () => {
    await sequelize.close();
  });

  it('Should create a product', async () => {
    const response = await request(app).post('/product').send({
      name: 'Laptop B',
      price: 1798.9,
      type: 'b',
    });

    expect(response.status).toBe(200);
    expect(response.body.name).toBe('Laptop B');
    expect(response.body.price).toBe(1798.9 * 2);
  });

  it('Should not create a product (invalid type)', async () => {
    const response = await request(app).post('/product').send({
      name: 'Laptop A',
      price: 1755.25,
      type: 'invalid',
    });
    expect(response.status).toBe(500);
  });

  it('Should not create a product (invalid price)', async () => {
    const response = await request(app).post('/product').send({
      name: 'Laptop A',
      price: -1755.25,
      type: 'a',
    });
    expect(response.status).toBe(500);
  });

  it('Should list all products', async () => {
    const responseCreateProduct1 = await request(app).post('/product').send({
      name: 'Laptop A',
      price: 1755.25,
      type: 'a',
    });
    expect(responseCreateProduct1.status).toBe(200);

    const responseCreateProduct2 = await request(app).post('/product').send({
      name: 'Laptop B',
      price: 1798.9,
      type: 'b',
    });
    expect(responseCreateProduct2.status).toBe(200);

    const responseListProducts = await request(app).get('/product').send();

    expect(responseListProducts.status).toBe(200);
    expect(responseListProducts.body.products.length).toBe(2);
    expect(responseListProducts.body.products[0].name).toBe('Laptop A');
    expect(responseListProducts.body.products[0].price).toBe(1755.25);
    expect(responseListProducts.body.products[1].name).toBe('Laptop B');
    expect(responseListProducts.body.products[1].price).toBe(1798.9 * 2);

    const responseListProductsXML = await request(app)
      .get('/product')
      .set('Accept', 'application/xml')
      .send();

    expect(responseListProductsXML.status).toBe(200);
    expect(responseListProductsXML.text).toContain(
      `<?xml version="1.0" encoding="UTF-8"?>`
    );
    expect(responseListProductsXML.text).toContain(`<products>`);
    expect(responseListProductsXML.text).toContain(`<product>`);
    expect(responseListProductsXML.text).toContain(`<name>Laptop A</name>`);
    expect(responseListProductsXML.text).toContain(`<price>1755.25</price>`);
    expect(responseListProductsXML.text).toContain(`<name>Laptop B</name>`);
    expect(responseListProductsXML.text).toContain(`<price>3597.8</price>`);
    expect(responseListProductsXML.text).toContain(`</product>`);
    expect(responseListProductsXML.text).toContain(`</products>`);
  });
});
