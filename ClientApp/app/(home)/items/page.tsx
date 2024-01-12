import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import Pagination from "@/components/common/Pagination/Pagination";
import { SearchParams } from "@/types/next";
import { Resource, newResource } from "@/types/resource";

type Item = {}

type Filters = {
    page: number;
    perPage: number;
    sort: string;
    order: string;
    search: string;
    category:  string;
}
type ItemProps = {
    resource: Resource<Item>;
    filters: Filters
}

const fetchItems = async (searchParams: SearchParams): Promise<ItemProps> => { 
  let page = 1
  if (searchParams?.page) {
    page = parseInt(searchParams.page.toString(), 10)
  }

  let perPage = 20
  if (searchParams?.per_page) {
    perPage = parseInt(searchParams.per_page.toString(), 10)
  }

  let sort = 'id'
  if (searchParams?.sort) {
    sort = searchParams.sort.toString()
  }

  let order = 'asc'
  if (searchParams?.order && typeof searchParams.order === 'string') {
    order = searchParams.order
  }

  let search = ""
  if(searchParams?.search  && typeof searchParams.search === 'string'){
    search = searchParams.search;
  }

  let category = ""
  if(searchParams?.category  && typeof searchParams.category === 'string'){
    category = searchParams.category;
  }

  const url = new URL('backend url')
  url.searchParams.set('_page', page.toString())
  url.searchParams.set('_limit', perPage.toString())
  url.searchParams.set('_sort', sort)
  url.searchParams.set('_order', order)
  url.searchParams.set("_search",  search)
  url.searchParams.set("_category",  category)

  const res = await fetch(url, {
    method: 'GET',
  })
  const items: Item[] = await res.json()

  const total = parseInt(res.headers.get('x-total-count') ?? '0', 10)
  const resource: Resource<Item> = newResource(items, total, page, perPage)

  const  filters = {
    page,
    perPage,
    sort,
    order,
    search,
    category
  }

  return {
    resource,
    filters
  }
}

export default async function  ItemsPage ({ searchParams }: { searchParams: SearchParams }) {
  // const itemProps = await fetchItems(searchParams);
  const resource = newResource([], 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);

  return (
    <>
       <h1>{JSON.stringify(searchParams)}</h1>
       <h1>{JSON.stringify(resource.meta)}</h1>
       <Pagination meta={resource.meta} />
    </>
  );
};

