import Breadcrumb from "@/components/Home/Breadcrumb";
import Pagination from "@/components/common/Pagination/Pagination";
import { category1, category2, item1, item2 } from "@/data/item";
import { Category } from "@/types/models/category";
import { SearchParams } from "@/types/next";
import { Resource, newResource } from "@/types/resource";
import { useRouter } from "next/router";
import { useState } from "react";
import Index from "./Index";
import { Item } from "@/types/models/item";

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

const items:Item[] = [
  item1, item2,
  item1, item2,
  item1, item2,
  item1, item2,
]

export default async function  ItemsPage ({ searchParams }: { searchParams: SearchParams }) {
  // const itemProps = await fetchItems(searchParams);

  const resource = newResource(items, 93, searchParams?.page ? parseInt(searchParams.page.toString(), 10) : 1, 10);

  return (
    <>
    <Breadcrumb listPages={[{
      pageName: "market",
      link: "/items"
    }]} />
    <Index searchParams={searchParams} resource={resource}/>
    </>
  );
};