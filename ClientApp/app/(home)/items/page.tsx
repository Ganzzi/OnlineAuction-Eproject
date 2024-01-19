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
import axios from "axios";
import https from 'https'
import { CategoryItem } from "@/types/models/categoryItem";

type Filters = {
    page: number;
    perPage: number;
    // sort: string;
    // order: string;
    search: string;
    category:  string;
}
type ItemProps = {
    resource: Resource<CategoryItem>;
    filters: Filters
}

const items:Item[] = [
  item1, item2,
  item1, item2,
  item1, item2,
  item1, item2,
]

const fetchItems = async (searchParams: SearchParams): Promise<ItemProps> => { 
  let page = 1
  if (searchParams?.page) {
    page = parseInt(searchParams.page.toString(), 10)
  }

  let perPage = 10
  if (searchParams?.per_page) {
    perPage = parseInt(searchParams.per_page.toString(), 10)
  }

  // let sort = 'id'
  // if (searchParams?.sort) {
  //   sort = searchParams.sort.toString()
  // }

  // let order = 'asc'
  // if (searchParams?.order && typeof searchParams.order === 'string') {
  //   order = searchParams.order
  // }

  let search = ""
  if(searchParams?.search  && typeof searchParams.search === 'string'){
    search = searchParams.search;
  }

  let category = ""
  if(searchParams?.category  && typeof searchParams.category === 'string'){
    category = searchParams.category;
  }

  const url = new URL('https://localhost:7073/api/User/ListItemsWithQuery')
  url.searchParams.set('page', page.toString())
  url.searchParams.set('take', perPage.toString())
  // url.searchParams.set('sort', sort)
  // url.searchParams.set('order', order)
  url.searchParams.set("search",  search)
  url.searchParams.set("cate",  category)

  const response = await axios.get(url.toString(), {
    httpsAgent: new https.Agent({ rejectUnauthorized: false }), // Ignore SSL certificate validation errors
  });

  const res = response.data;

  const items: CategoryItem[] = res?.listSearch;
  const total = res?.count;  

  const resource: Resource<CategoryItem> = newResource(items, total, page, perPage)

  const  filters = {
    page,
    perPage,
    search,
    category
    // sort,
    // order,
  }

  return {
    resource,
    filters
  }
}


export default async function  ItemsPage ({ searchParams }: { searchParams: SearchParams }) {
  const itemProps = await fetchItems(searchParams);

  return (
    <>
    <Breadcrumb listPages={[{
      pageName: "market",
      link: "/items"
    }]} />
    <Index searchParams={searchParams} resource={itemProps.resource}/>
    </>
  );
};