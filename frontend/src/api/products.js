
const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5277";

export async function getProducts(params = {}, signal) {
  const query = new URLSearchParams();

  if (params.search?.trim()) {
    query.set("search", params.search.trim());
  }

  if (params.categoryId) {
    query.set("categoryId", params.categoryId);
  }

  query.set("sortBy", params.sortBy || "id");
  query.set("sortOrder", params.sortOrder || "asc");
  query.set("page", String(params.page || 1));
  query.set("pageSize", String(params.pageSize || 8));

  const response = await fetch(
    `${API_BASE_URL}/api/v1/products?${query.toString()}`,
    { signal }
  );

  if (!response.ok) {
    throw new Error(
      `Unable to load products (HTTP ${response.status}).`
    );
  }

  return response.json();
}
