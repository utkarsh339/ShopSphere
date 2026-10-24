import "./App.css";

const products = [
  {
    id: 1,
    name: "Wireless Headphones",
    category: "Electronics",
    price: 2499,
    icon: "🎧",
    badge: "BESTSELLER",
  },
  {
    id: 2,
    name: "Smart Watch",
    category: "Electronics",
    price: 3999,
    icon: "⌚",
    badge: "POPULAR",
  },
  {
    id: 3,
    name: "Everyday Backpack",
    category: "Accessories",
    price: 1499,
    icon: "🎒",
    badge: "",
  },
  {
    id: 4,
    name: "Running Shoes",
    category: "Fashion",
    price: 2999,
    icon: "👟",
    badge: "NEW",
  },
];

function Header() {
  return (
    <header className="site-header">
      <a className="brand" href="/" aria-label="ShopSphere home">
        Shop<span>Sphere</span>
      </a>

      <nav className="main-nav" aria-label="Main navigation">
        <a href="#home">Home</a>
        <a href="#categories">Categories</a>
        <a href="#products">Products</a>
      </nav>

      <button className="cart-button" type="button">
        🛒 Cart <span className="cart-count">0</span>
      </button>
    </header>
  );
}

function Hero() {
  return (
    <section className="hero" id="home">
      <div className="hero-content">
        <span className="eyebrow">YOUR EVERYDAY MARKETPLACE</span>
        <h1>Find your next favorite thing.</h1>
        <p>
          Discover quality products across electronics, fashion, and everyday
          essentials.
        </p>
        <a className="primary-button" href="#products">
          Explore products →
        </a>
      </div>

      <div className="hero-art" aria-hidden="true">
        <span>🛍️</span>
        <div className="floating-item item-one">🎧</div>
        <div className="floating-item item-two">👟</div>
        <div className="floating-item item-three">⌚</div>
      </div>
    </section>
  );
}

function Categories() {
  const categories = [
    { name: "Electronics", icon: "💻" },
    { name: "Fashion", icon: "👕" },
    { name: "Accessories", icon: "🎒" },
    { name: "Home & Living", icon: "🏠" },
  ];

  return (
    <section className="section" id="categories">
      <div className="section-heading">
        <div>
          <span className="eyebrow">BROWSE BY INTEREST</span>
          <h2>Explore categories</h2>
        </div>
      </div>

      <div className="category-grid">
        {categories.map((category) => (
          <a className="category-card" href="#products" key={category.name}>
            <span className="category-icon">{category.icon}</span>
            <span>{category.name}</span>
            <span className="category-arrow">↗</span>
          </a>
        ))}
      </div>
    </section>
  );
}

function ProductCard({ product }) {
  return (
    <article className="product-card">
      <div className="product-art">
        {product.badge && (
          <span className="product-badge">{product.badge}</span>
        )}
        <span className="product-icon" aria-hidden="true">
          {product.icon}
        </span>
        <button
          className="wishlist-button"
          type="button"
          aria-label={`Add ${product.name} to wishlist`}
        >
          ♡
        </button>
      </div>

      <div className="product-details">
        <span className="product-category">{product.category}</span>
        <h3>{product.name}</h3>

        <div className="product-bottom">
          <strong>
            {product.price.toLocaleString("en-IN", {
              style: "currency",
              currency: "INR",
              maximumFractionDigits: 0,
            })}
          </strong>
          <button
            className="add-button"
            type="button"
            aria-label={`Add ${product.name} to cart`}
          >
            +
          </button>
        </div>
      </div>
    </article>
  );
}

function FeaturedProducts() {
  return (
    <section className="section" id="products">
      <div className="section-heading">
        <div>
          <span className="eyebrow">PICKED FOR YOU</span>
          <h2>Featured products</h2>
        </div>
        <a className="text-link" href="#products">
          View all →
        </a>
      </div>

      <div className="product-grid">
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </section>
  );
}

function Footer() {
  return (
    <footer className="site-footer">
      <a className="brand" href="#home">
        Shop<span>Sphere</span>
      </a>
      <p>Thoughtful shopping, made simple.</p>
      <p>© {new Date().getFullYear()} ShopSphere</p>
    </footer>
  );
}

function App() {
  return (
    <>
      <Header />
      <main>
        <Hero />
        <Categories />
        <FeaturedProducts />
      </main>
      <Footer />
    </>
  );
}

export default App;
