import { useEffect, useState } from "react";
import { getProducts } from "./api/products";
import "./App.css";

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
        <span className="product-category">{product.categoryName}</span>
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

function FeaturedProducts({
  products,
  loading,
  error,
  onRetry,
  searchInput,
  setSearchInput,
  onSearch,
  categoryId,
  setCategoryId,
  sortBy,
  setSortBy,
  sortOrder,
  setSortOrder,
  page,
  totalPages,
  totalCount,
  onPageChange,
}) {
  return (
    <section className="section" id="products">
      <div className="section-heading">
        <div>
          <span className="eyebrow">PICKED FOR YOU</span>
          <h2>Featured products</h2>
        </div>
      </div>

      <form onSubmit={onSearch} className="catalog-controls">
        <input
          type="search"
          aria-label="Search products"
          placeholder="Search products..."
          value={searchInput}
          onChange={(event) => setSearchInput(event.target.value)}
        />

        <button type="submit">Search</button>

        <select
          aria-label="Filter by category"
          value={categoryId}
          onChange={(event) => setCategoryId(event.target.value)}
        >
          <option value="">All categories</option>
          <option value="1">Electronics</option>
          <option value="2">Accessories</option>
          <option value="3">Fashion</option>
        </select>

        <select
          aria-label="Sort products by"
          value={sortBy}
          onChange={(event) => setSortBy(event.target.value)}
        >
          <option value="id">Default order</option>
          <option value="name">Name</option>
          <option value="price">Price</option>
        </select>

        <select
          aria-label="Sort direction"
          value={sortOrder}
          onChange={(event) => setSortOrder(event.target.value)}
        >
          <option value="asc">Ascending</option>
          <option value="desc">Descending</option>
        </select>
      </form>

      <p aria-live="polite">
        {loading ? "Loading products..." : `${totalCount} product(s) found`}
      </p>

      {loading && <p role="status">Loading products...</p>}

      {!loading && error && (
        <div role="alert">
          <p>{error}</p>
          <button type="button" onClick={onRetry}>
            Try again
          </button>
        </div>
      )}

      {!loading && !error && products.length === 0 && (
        <p>No products match your search.</p>
      )}

      {!loading && !error && products.length > 0 && (
        <div className="product-grid">
          {products.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      )}

      {!loading && !error && totalPages > 0 && (
        <div className="pagination-controls">
          <button
            type="button"
            disabled={page <= 1}
            onClick={() => onPageChange(page - 1)}
          >
            Previous
          </button>

          <span>
            Page {page} of {totalPages}
          </span>

          <button
            type="button"
            disabled={page >= totalPages}
            onClick={() => onPageChange(page + 1)}
          >
            Next
          </button>
        </div>
      )}
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
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [searchInput, setSearchInput] = useState("");
  const [search, setSearch] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [sortBy, setSortBy] = useState("id");
  const [sortOrder, setSortOrder] = useState("asc");

  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [totalCount, setTotalCount] = useState(0);
  const [retryCount, setRetryCount] = useState(0);

  useEffect(() => {
    const controller = new AbortController();

    async function loadProducts() {
      setLoading(true);
      setError("");

      try {
        const result = await getProducts(
          {
            search,
            categoryId,
            sortBy,
            sortOrder,
            page,
            pageSize: 8,
          },
          controller.signal,
        );

        setProducts(result.items);
        setTotalPages(result.totalPages);
        setTotalCount(result.totalCount);
      } catch (err) {
        if (err.name === "AbortError") return;

        setError(err.message || "Unable to load products.");
      } finally {
        if (!controller.signal.aborted) {
          setLoading(false);
        }
      }
    }

    loadProducts();

    return () => controller.abort();
  }, [search, categoryId, sortBy, sortOrder, page, retryCount]);

  function handleSearch(event) {
    event.preventDefault();
    setPage(1);
    setSearch(searchInput.trim());
  }

  function handleCategoryChange(value) {
    setCategoryId(value);
    setPage(1);
  }

  function handleSortByChange(value) {
    setSortBy(value);
    setPage(1);
  }

  function handleSortOrderChange(value) {
    setSortOrder(value);
    setPage(1);
  }

  function handleRetry() {
    setRetryCount((count) => count + 1);
  }

  return (
    <>
      <Header />

      <main>
        <Hero />
        <Categories />

        <FeaturedProducts
          products={products}
          loading={loading}
          error={error}
          onRetry={handleRetry}
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onSearch={handleSearch}
          categoryId={categoryId}
          setCategoryId={handleCategoryChange}
          sortBy={sortBy}
          setSortBy={handleSortByChange}
          sortOrder={sortOrder}
          setSortOrder={handleSortOrderChange}
          page={page}
          totalPages={totalPages}
          totalCount={totalCount}
          onPageChange={setPage}
        />
      </main>

      <Footer />
    </>
  );
}

export default App;
