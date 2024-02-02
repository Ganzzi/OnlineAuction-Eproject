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
import { getListCategories } from "./form/page";

type Filters = {
    page: number;
    perPage: number;
    // sort: string;
    // order: string;
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

  let perPage = 10
  if (searchParams?.per_page) {
    perPage = parseInt(searchParams.per_page.toString(), 10)
  }

  let order = 'title'
  if (searchParams?.order) {
    order = searchParams.order.toString()
  }

  let search = ""
  if(searchParams?.search  && typeof searchParams.search === 'string'){
    search = searchParams.search;
  }

  let cate = ""
  if(searchParams?.cate  && typeof searchParams.cate === 'string'){
    cate = searchParams.cate;
  }

  const url = new URL(process.env.NEXT_PUBLIC_SERVER_URL+'/api/User/ListItemsWithQuery')
  url.searchParams.set('page', page.toString())
  url.searchParams.set('take', perPage.toString())
  url.searchParams.set('order', order)
  url.searchParams.set("search",  search)
  url.searchParams.set("cate",  cate)

  const response = await axios.get(url.toString(), {
    httpsAgent: new https.Agent({ rejectUnauthorized: false }), // Ignore SSL certificate validation errors
  });

  const res = response.data;  

  const items: Item[] = res?.listSearch;
  const total = res?.count;  

  const resource: Resource<Item> = newResource(items, total, page, perPage)

  const  filters = {
    page,
    perPage,
    search,
    category: cate
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
  const categories = await getListCategories();

  return (
    <>
    <Breadcrumb listPages={[{
      pageName: "market",
      link: "/items"
    }]} />
    <Index searchParams={searchParams} resource={itemProps.resource} categories={categories}/>
    </>
  );
};