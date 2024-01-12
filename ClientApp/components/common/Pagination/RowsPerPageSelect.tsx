'use client'

import React from 'react'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'

type Props = {
  perPage: number;
  setPerPage?: (perPage: number) => void;
}

export default function RowPerPageSelect(props: Props) {
  const { perPage, setPerPage } = props
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  return (
    <></>
  )
}
